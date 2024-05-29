using Core.HttpLogic.Services.Interfaces;
using Core.HttpLogic.Types;
using Core.RabbitRpc.Services.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfileConnection.Dto.GetProfiles;
using ProfileConnection.Interfaces;

namespace ProfileConnection;

public class ProfileConnectionService : IProfileConnectionService
{
	private readonly IHttpRequestService? _httpRequestService = null!;
	private readonly ProfileConnectionOptions _profileConnectionOptions;
	private readonly IRabbitPublisher? _rabbitPublisher;

	public ProfileConnectionService(IServiceProvider serviceProvider, ProfileConnectionOptions profileConnectionOptions)
	{
		this._profileConnectionOptions = profileConnectionOptions;

		if (profileConnectionOptions.ConnectionType == "http")
			this._httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
		if (profileConnectionOptions.ConnectionType == "rpc")
			this._rabbitPublisher = serviceProvider.GetRequiredService<IRabbitPublisher>();
	}

	public async Task<Result<GetProfilesByIdResponse>> GetProfiles(GetProfilesByIdRequest request, string replyQueue)
	{
		if (this._profileConnectionOptions.ConnectionType == "http")
		{
			var response = await this._httpRequestService!.SendRequestAsync<GetProfilesByIdResponse>(
				new HttpRequestData()
				{
					Uri = new Uri($"{this._profileConnectionOptions.Url}/api/profile"), // Todo узнать какой url,
					Method = HttpMethod.Get,
					ContentType = ContentType.ApplicationJson,
					Body = request.Ids
				});

			if (!response.IsSuccessStatusCode || response.Body == null)
				return Result.Failure<GetProfilesByIdResponse>("Запрос на получения профилей вернул ошибку");

			return response.Body;
		}

		if (this._profileConnectionOptions.ConnectionType == "rpc")
		{
			if (this._rabbitPublisher == null)
				throw new ArgumentNullException(nameof(this._rabbitPublisher));

			_ = this._rabbitPublisher.QueueDeclare(replyQueue,
				exclusive: false,
				durable: true,
				autoDelete: false,
				arguments: null);

			var result = this._rabbitPublisher.Call<GetProfilesByIdResponse>(request, this._profileConnectionOptions.QueueServiceName!, replyQueue);
			return result;
		}

		throw new NotImplementedException();
	}
}