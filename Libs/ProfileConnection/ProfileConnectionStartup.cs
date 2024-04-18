using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfileConnection.Interfaces;

namespace ProfileConnection;

/// <summary>
/// Класс регистрации сервисов для соедния
/// </summary>
public static class ProfileConnectionStartup
{
	public static IServiceCollection AddProfileConnectionServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<IProfileConnectionService, ProfileConnectionService>();

		services.Configure<ProfileConnectionOptions>(configuration.GetSection(nameof(ProfileConnectionOptions)));

		return services;
	}
}