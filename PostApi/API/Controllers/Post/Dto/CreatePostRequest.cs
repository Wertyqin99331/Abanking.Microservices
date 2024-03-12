using System.ComponentModel.DataAnnotations;

namespace API.Controllers.Post.Dto;

public record CreatePostRequest(
	[Required] string Title,
	[Required] string Text);