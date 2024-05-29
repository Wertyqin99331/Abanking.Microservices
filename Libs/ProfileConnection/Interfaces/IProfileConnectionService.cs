using CSharpFunctionalExtensions;
using ProfileConnection.Dto.GetProfiles;

namespace ProfileConnection.Interfaces;

public interface IProfileConnectionService
{
	/// <summary>
	/// Получить профили пользователей по id
	/// </summary>
	/// <param name="request">Запрос</param>
	/// <param name="replyQueue">Имя очереди</param>
	/// <returns>Результат получения</returns>
	Task<Result<GetProfilesByIdResponse>> GetProfiles(GetProfilesByIdRequest request, string replyQueue);
}