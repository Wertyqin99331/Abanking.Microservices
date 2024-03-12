using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Core.Logging;

public static class DependencyInjection
{
	public static ConfigureHostBuilder AddLoggingServices(this ConfigureHostBuilder host)
	{
		host.UseSerilog((context, config) =>
		{
			config.ReadFrom.Configuration(context.Configuration);
		});

		return host;
	}
}