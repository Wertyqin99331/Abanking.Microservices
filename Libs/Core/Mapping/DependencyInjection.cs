using Core.Helpers;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Mapping;

public static class DependencyInjection
{
	public static IServiceCollection AddMappingServices(this IServiceCollection services)
	{
		var config = TypeAdapterConfig.GlobalSettings;
		config.Scan(AssemblyHelper.GetAllAssembliesWithoutDefaultAssemblies().ToArray());
		services.AddSingleton(config);
		services.AddScoped<IMapper, Mapper>();

		return services;
	}
}