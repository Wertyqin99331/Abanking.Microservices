using Application.Managers.Post.Dto;
using Domain.Entities;
using Mapster;
using ProfileConnection.Dto.GetProfiles;

namespace Application.Mapping;

public sealed class PostMappingConfiguration: IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<(Post Post, GetProfilesDto User), GetPostByIdResponse>()
			.Map(dest => dest.Id, src => src.Post.Id)
			.Map(dest => dest.UserId, src => src.Post.UserId)
			.Map(dest => dest.Name, src => src.User.Name)
			.Map(dest => dest.Surname, src => src.User.Surname)
			.Map(dest => dest.Title, src => src.Post.Title)
			.Map(dest => dest.Text, src => src.Post.Text)
			.Map(dest => dest.DateCreated, src => src.Post.DateCreated);
		
		config.NewConfig<Post, PostDto>()
			.Map(dest => dest.Title, src => src.Title.Value)
			.Map(dest => dest.Text, src => src.Text.Value);
	}
}