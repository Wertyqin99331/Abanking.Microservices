using Application.Managers.Like;
using Core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Like;

[ApiController]
[Route("api/post/like")]
[Produces("application/json")]
public class LikeController(ILikeManager likeManager): ControllerBase
{
	/// <summary>
	/// Поставить или убрать лайк с поста
	/// </summary>
	/// <param name="postId">Id поста</param>
	/// <returns>Результат</returns>
	[HttpPost("{postId:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ToggleLike([FromRoute] Guid postId)
	{
		var result = await likeManager.ToggleLike(postId);

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: NoContent();
	}
}