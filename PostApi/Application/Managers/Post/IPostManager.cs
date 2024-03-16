using Application.Managers.Post.Dto;
using CSharpFunctionalExtensions;

namespace Application.Managers.Post;

public interface IPostManager
{
	/// <summary>
	/// Создать пост
	/// </summary>
	/// <param name="body">Тело создания поста</param>
	/// <returns>Результат создания поста</returns>
	Task<Result> CreatePost(CreatePostBody body);

	/// <summary>
	/// Получить пост по id
	/// </summary>
	/// <param name="id">Id поста</param>
	/// <returns>Результат получения</returns>
	Task<Result<GetPostByIdResponse>> GetPostById(Guid id);
	
	/// <summary>
	/// Получить посты
	/// </summary>
	/// <param name="page">Номер страницы</param>
	/// <param name="countPerPage">Количество постов на странице</param>
	/// <param name="filter">Функция фильтрации</param>
	/// <returns>Результат получения</returns>
	Task<Result<GetPostsResponse>> GetPosts(int page, int countPerPage, Func<Domain.Entities.Post, bool>? filter = null);

	/// <summary>
	/// Удалить пост
	/// </summary>
	/// <param name="id">Id поста</param>
	/// <returns>Результат удаления</returns>
	Task<Result> DeletePost(Guid id);

	/// <summary>
	/// Обновить пост по id
	/// </summary>
	/// <param name="id">Id поста</param>
	/// <param name="body">Тело обновления</param>
	/// <returns></returns>
	Task<Result<GetPostByIdResponse>> UpdatePost(Guid id, UpdatePostBody body);
}