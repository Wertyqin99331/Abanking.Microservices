using System.Net.Mime;
using Core.DbEntity;
using CSharpFunctionalExtensions;
using Domain.ValueObjects.Post;

namespace Domain.Entities;

public class Post : IDbEntity<Guid>
{
	public required Guid Id { get; init; }
	public required Guid UserId { get; init; }
	public required Title Title { get; set; }
	public required Text Text { get; set; }
	public required DateTime DateCreated { get; init; }

	public List<Like> Likes { get; set; } = [];

	public static Result<Post> Create(Guid id, Guid userId, string title, string text)
	{
		// Todo Check that user with this id exists

		var titleResult = Title.Create(title);
		if (titleResult.IsFailure)
			return Result.Failure<Post>(titleResult.Error);

		var textResult = Text.Create(text);
		if (textResult.IsFailure)
			return Result.Failure<Post>(textResult.Error);

		return new Post
		{
			Id = id,
			UserId = userId,
			Title = titleResult.Value,
			Text = textResult.Value,
			DateCreated = DateTime.UtcNow
		};
	}

	public Result Update(string title, string text)
	{
		var titleResult = Title.Create(title);
		if (titleResult.IsFailure)
			return Result.Failure(titleResult.Error);

		var textResult = Text.Create(text);
		if (textResult.IsFailure)
			return Result.Failure(textResult.Error);

		this.Title = titleResult.Value;
		this.Text = textResult.Value;

		return Result.Success();
	}
}