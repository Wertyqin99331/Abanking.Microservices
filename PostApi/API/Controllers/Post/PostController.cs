using API.Controllers.Post.Dto;
using Application.Managers.Post;
using Application.Managers.Post.Dto;
using Core.Dto;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using GetPostByIdResponse = API.Controllers.Post.Dto.GetPostByIdResponse;
using GetPostsResponse = API.Controllers.Post.Dto.GetPostsResponse;

namespace API.Controllers.Post;

[ApiController]
[Route("api/post")]
[Produces("application/json")]
public class PostController(IPostManager postManager, IMapper mapper) : ControllerBase
{
	/// <summary>
	/// Создать пост
	/// </summary>
	/// <param name="request">Запрос</param>
	/// <returns>Результат создания</returns>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
	{
		var result = await postManager.CreatePost(mapper.Map<CreatePostBody>(request));

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: NoContent();
	}

	/// <summary>
	/// Получить пост по id
	/// </summary>
	/// <param name="id">Id поста</param>
	/// <returns>Результат получения</returns>
	[HttpGet("{id:guid}")]
	[ProducesResponseType<GetPostByIdResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetPostById([FromRoute] Guid id)
	{
		var result = await postManager.GetPostById(id);

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: Ok(mapper.Map<GetPostByIdResponse>(result.Value));
	}
	
	/// <summary>
	/// Получить посты
	/// </summary>
	/// <param name="page">Номер страницы</param>
	/// <param name="countPerPage">Количество постов на странице</param>
	/// <returns>Результат</returns>
	[HttpGet]
	[ProducesResponseType<GetPostsResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetPosts([FromQuery] int page = 1, [FromQuery] int countPerPage = 10)
	{
		var result = await postManager.GetPosts(page, countPerPage);

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: Ok(mapper.Map<GetPostsResponse>(result.Value));
	}
	
	/// <summary>
	/// Удалить пост по id
	/// </summary>
	/// <param name="id">Id поста</param>
	/// <returns>Результат удаления</returns>
	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> DeletePost([FromRoute] Guid id)
	{
		var result = await postManager.DeletePost(id);

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: NoContent();
	}

	/// <summary>
	/// Обновить пост по id
	/// </summary>
	/// <param name="id">Id поста</param>
	/// <param name="request">Тело запроса</param>
	/// <returns>Результат обновления</returns>
	[HttpPut("{id:guid}")]
	[ProducesResponseType<GetPostByIdResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdatePost([FromRoute] Guid id, [FromBody] UpdatePostRequest request)
	{
		var result = await postManager.UpdatePost(id, mapper.Map<UpdatePostBody>(request));

		return result.IsFailure
			? BadRequest(new ErrorResponse(result.Error))
			: Ok(mapper.Map<GetPostByIdResponse>(result.Value));
	}
}