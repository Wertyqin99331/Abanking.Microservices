using Application.Managers.Post;
using Application.Managers.Post.Dto;
using Domain.Entities;
using Mapster;

namespace Application.Mapping;

public sealed class PostMappingConfiguration: IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Post, GetPostByIdResponse>()
			.Map(dest => dest.Title, src => src.Title.Value)
			.Map(dest => dest.Text, src => src.Text.Value);

		config.NewConfig<Post, PostDto>()
			.Map(dest => dest.Title, src => src.Title.Value)
			.Map(dest => dest.Text, src => src.Text.Value);
	}
}