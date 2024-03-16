using CSharpFunctionalExtensions;

namespace Application.Managers.Like;

public interface ILikeManager
{
	/// <summary>
	/// Поставить или удалить лайк с поста
	/// </summary>
	/// <param name="postId">Id поста</param>
	/// <returns>Результат</returns>
	public Task<Result> ToggleLike(Guid postId);
}