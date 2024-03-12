using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Middlewares;

public class OperationCanceledMiddleware(ILogger<OperationCanceledMiddleware> logger): IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (OperationCanceledException)
		{
			logger.LogInformation("Request was canceled");
			context.Response.StatusCode = 409;
		}
	}
}

public static class OperationCanceledMiddlewareExtensions
{
	public static IServiceCollection AddOperationCanceledMiddlewareServices(this IServiceCollection services)
	{
		services.AddTransient<OperationCanceledMiddleware>();

		return services;
	}

	public static WebApplication UseOperationCanceledMiddleware(this WebApplication app)
	{
		app.UseMiddleware<OperationCanceledMiddleware>();

		return app;
	}
}