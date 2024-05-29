using System.Text;
using Core.RabbitRpc.Helpers;
using Core.RabbitRpc.Services.Interfaces;
using Logic.Managers.Profile;
using Logic.Options;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProfileConnection.Dto.GetProfiles;
using RabbitMQ.Client.Events;
using IModel = RabbitMQ.Client.IModel;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Logic.Services;

public class RpcServerService(
	IPooledObjectPolicy<IModel> objectPolicy,
	IOptions<RabbitConnectionOptions> options,
	IServiceProvider serviceProvider,
	ILogger<RpcServerService> logger,
	IOptions<RpcOptions> rpcOptions) : IHostedService
{
	private readonly DefaultObjectPool<IModel> _objectPool = new(objectPolicy, options.Value.MaxChannelCount);

	public Task StartAsync(CancellationToken cancellationToken)
	{
		var channel = new PoolObject<IModel>(this._objectPool).Item;
		var consumer = new AsyncEventingBasicConsumer(channel);

		consumer.Received += async (_, ea) =>
		{
			var props = ea.BasicProperties;
			var request = JsonSerializer.Deserialize<GetProfilesByIdRequest>(ea.Body.ToArray());
			if (request == null)
			{
				channel.BasicAck(ea.DeliveryTag, false);
				return;
			}

			logger.LogInformation($"CorrelationId: {props.CorrelationId}");
			using var serviceScope = serviceProvider
				.GetRequiredService<IServiceScopeFactory>()
				.CreateScope();
			var profileManager = serviceScope.ServiceProvider.GetRequiredService<IProfileManager>();

			var usersResult = await profileManager.GetProfiles(request);
			if (usersResult.IsFailure)
			{
				channel.BasicAck(ea.DeliveryTag, false);
				return;
			}
			
			var rabbitPublisher = serviceProvider.GetRequiredService<IRabbitPublisher>();
			rabbitPublisher.Publish(usersResult.Value, string.Empty, props.ReplyTo, string.Empty, props.CorrelationId);

			channel.BasicAck(ea.DeliveryTag, false);
		};

		channel.BasicConsume(rpcOptions.Value.QueueName, false, rpcOptions.Value.QueueName, false, false, null,
			consumer);

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}