namespace Application.Managers.Post.Dto;

public record GetPostsResponse
{
	public required List<PostDto> Posts { get; init; }
	public required bool IsNextPageExists { get; init; }
}