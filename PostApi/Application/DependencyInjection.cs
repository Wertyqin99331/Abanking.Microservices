using Application.Managers.Like;
using Application.Managers.Post;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddScoped<IPostManager, PostManager>();
		services.AddScoped<ILikeManager, LikeManager>();
		
		return services;
	}
}