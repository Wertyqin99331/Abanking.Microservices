using System.Linq.Expressions;

namespace API.Controllers.Post.Dto;

public record GetPostsResponse(List<PostDto> Posts, bool IsNextPageExists);