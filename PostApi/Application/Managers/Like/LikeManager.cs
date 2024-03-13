using Application.Abstractions;
using Core.Authentication;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Application.Managers.Like;

internal class LikeManager(IAuthenticationHelper authenticationHelper, IPostDbContext dbContext) : ILikeManager
{
	public async Task<Result> ToggleLike(Guid postId)
	{
		var userIdResult = authenticationHelper.GetUserId();
		if (userIdResult.IsFailure)
			return Result.Failure(userIdResult.Error);

		var post = await dbContext.Posts
			.Include(p => p.Likes)
			.FirstOrDefaultAsync(p => p.Id == postId);
		if (post is null)
			return Result.Failure("Нет поста с таким id");

		var existingLike = post.Likes.FirstOrDefault(l => l.UserId == userIdResult.Value);
		if (existingLike is null)
		{
			var likeResult = Domain.Entities.Like.Create(userIdResult.Value, post.Id);
			if (likeResult.IsFailure)
				return Result.Failure(likeResult.Error);

			await dbContext.Likes.AddAsync(likeResult.Value);
		}
		else
			dbContext.Likes.Remove(existingLike);

		await dbContext.SaveChangesAsync();

		return Result.Success();
	}
}