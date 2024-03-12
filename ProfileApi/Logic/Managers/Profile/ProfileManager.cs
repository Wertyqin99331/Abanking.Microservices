using Core.Authentication;
using CSharpFunctionalExtensions;
using Logic.Managers.Profile.Dto;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Logic.Managers.Profile;

public class ProfileManager(ProfileDbContext dbContext, IAuthenticationHelper authenticationHelper, IMapper mapper) : IProfileManager
{
	public async Task<Result> Register(RegisterBody body)
	{
		var (_, profileFailure, profile, profileError) =
			Persistence.Entities.Profile.Create(Guid.NewGuid(), body.Email, body.Name, body.Surname);
		if (profileFailure)
			return Result.Failure(profileError);

		await dbContext.Profiles.AddAsync(profile);
		await dbContext.SaveChangesAsync();

		return Result.Success();
	}

	public async Task<Result> DeleteProfile()
	{
		var userIdResult = authenticationHelper.GetUserId();
		if (userIdResult.IsFailure)
			return Result.Failure("Не удалось прочитать user id");

		var user = await dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == userIdResult.Value);
		if (user is null)
			return Result.Failure("Не удалось найти профиль с этим id");

		dbContext.Profiles.Remove(user);
		await dbContext.SaveChangesAsync();

		return Result.Success();

	}

	public async Task<Result<GetProfileResponse>> GetProfile()
	{
		var (_, userIdFailure, userId, userIdError) = authenticationHelper.GetUserId();
		if (userIdFailure)
			return Result.Failure<GetProfileResponse>(userIdError);

		var profile = await dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == userId);
		return profile is null
			? Result.Failure<GetProfileResponse>("Нет пользователя с таким id")
			: mapper.Map<GetProfileResponse>(profile);
	}

	public async Task<Result<GetProfileByIdResponse>> GetProfileById(Guid id)
	{
		var profile = await dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == id);

		return profile is null
			? Result.Failure<GetProfileByIdResponse>("Нет пользователя с таким id")
			: mapper.Map<GetProfileByIdResponse>(profile);
	}

	public async Task<Result> UpdateProfile(UpdateProfileBody body)
	{
		var userIdResult = authenticationHelper.GetUserId();
		if (userIdResult.IsFailure)
			return Result.Failure(userIdResult.Error);

		var profile = await dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == userIdResult.Value);
		if (profile is null)
			return Result.Failure("Нет пользователя с таким id");

		var updateResult = profile.UpdateProfile(body.Name, body.Surname);
		if (updateResult.IsFailure)
			return Result.Failure(updateResult.Error);

		await dbContext.SaveChangesAsync();

		return Result.Success();
	}
}