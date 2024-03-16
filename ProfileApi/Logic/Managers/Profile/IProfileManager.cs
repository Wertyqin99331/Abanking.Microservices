using CSharpFunctionalExtensions;
using Logic.Managers.Profile.Dto;

namespace Logic.Managers.Profile;

public interface IProfileManager
{
	/// <summary>
	/// Зарегистрировать пользователя
	/// </summary>
	/// <param name="body">Данные регистрации</param>
	/// <returns>Результат регистрации</returns>
	Task<Result> Register(RegisterBody body);
	
	/// <summary>
	/// Удалить пользователя
	/// </summary>
	/// <returns>Результат удаления</returns>
	Task<Result> DeleteProfile();
	
	/// <summary>
	/// Получить профиль пользователя
	/// </summary>
	/// <returns></returns>
	Task<Result<GetProfileResponse>> GetProfile();

	/// <summary>
	/// Получить профиль по id
	/// </summary>
	/// <returns></returns>
	Task<Result<GetProfileByIdResponse>> GetProfileById(Guid id);

	/// <summary>
	/// Обновить свой профиль
	/// </summary>
	/// <param name="newName"></param>
	/// <param name="newSurname"></param>
	/// <returns></returns>
	Task<Result> UpdateProfile(UpdateProfileBody body);
}