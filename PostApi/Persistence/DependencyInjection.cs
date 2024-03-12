using Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<PostDbContext>(options =>
		{
			options.UseNpgsql(configuration.GetConnectionString("Db"));
		});

		services.AddScoped<IPostDbContext>(options => options.GetRequiredService<PostDbContext>());
		
		return services;
	}
}