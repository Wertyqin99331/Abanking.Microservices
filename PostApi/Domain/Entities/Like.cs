using Core.DbEntity;
using CSharpFunctionalExtensions;

namespace Domain.Entities;

public class Like: IDbEntity
{
	private Like() {}
	
	public required Guid UserId { get; init; }
	public required Guid PostId { get; init; }
	public Post Post { get; init; } = null!;

	public static Result<Like> Create(Guid userId, Guid postId)
	{
		return new Like
		{
			UserId = userId,
			PostId = postId
		};
	}
}