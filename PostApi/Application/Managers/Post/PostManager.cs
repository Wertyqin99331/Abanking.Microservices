using System.Transactions;
using Application.Abstractions;
using Application.Managers.Post.Dto;
using Core.Authentication;
using CSharpFunctionalExtensions;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Managers.Post;

internal class PostManager(IPostDbContext dbContext, IAuthenticationHelper authenticationHelper, IMapper mapper)
	: IPostManager
{
	public async Task<Result> CreatePost(CreatePostBody body)
	{
		var userIdResult = authenticationHelper.GetUserId();
		if (userIdResult.IsFailure)
			return Result.Failure(userIdResult.Error);

		var postResult = Domain.Entities.Post.Create(Guid.NewGuid(), userIdResult.Value, body.Title, body.Text);
		if (postResult.IsFailure)
			return Result.Failure(postResult.Error);

		await dbContext.Posts.AddAsync(postResult.Value);
		await dbContext.SaveChangesAsync();

		return Result.Success();
	}

	public async Task<Result<GetPostByIdResponse>> GetPostById(Guid id)
	{
		var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);

		return post is null
			? Result.Failure<GetPostByIdResponse>("Нет поста с таким id")
			: mapper.Map<GetPostByIdResponse>(post);
	}

	public async Task<Result<GetPostsResponse>> GetPosts(int page, int countPerPage,
		Func<Domain.Entities.Post, bool>? filter = null)
	{
		var query = dbContext.Posts
			.OrderBy(p => p.DateCreated)
			.Skip((page - 1) * countPerPage)
			.Take(countPerPage);

		if (filter is not null)
			query = query.Where(p => filter(p));

		var posts = await query.ProjectToType<PostDto>().ToListAsync();
		return new GetPostsResponse
		{
			Posts = posts,
			IsNextPageExists = dbContext.Posts.Count() > page * countPerPage
		};
	}

	public async Task<Result> DeletePost(Guid id)
	{
		var (_, userIdFailure, userId, userIdError) = authenticationHelper.GetUserId();
		if (userIdFailure)
			return Result.Failure(userIdError);

		var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
		if (post is null)
			return Result.Failure("Нет поста с таким id");

		if (post.UserId != userId)
			return Result.Failure("У вас нет прав на удаление этого поста");

		dbContext.Posts.Remove(post);
		await dbContext.SaveChangesAsync();

		return Result.Success();
	}

	public async Task<Result<GetPostByIdResponse>> UpdatePost(Guid id, UpdatePostBody body)
	{
		var userIdResult = authenticationHelper.GetUserId();
		if (userIdResult.IsFailure)
			return Result.Failure<GetPostByIdResponse>(userIdResult.Error);
		

		var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
		if (post is null)
			return Result.Failure<GetPostByIdResponse>("Нет поста с таким id");

		if (post.UserId != userIdResult.Value)
			return Result.Failure<GetPostByIdResponse>("У вас нет прав для редактирования этого поста");

		var updateResult = post.Update(body.Title, body.Text);
		if (updateResult.IsFailure)
			return Result.Failure<GetPostByIdResponse>(updateResult.Error);

		await dbContext.SaveChangesAsync();

		return mapper.Map<GetPostByIdResponse>(post);
	}
}