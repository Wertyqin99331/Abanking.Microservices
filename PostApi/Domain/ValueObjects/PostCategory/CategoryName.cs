using Core.Extensions;
using CSharpFunctionalExtensions;

namespace Domain.ValueObjects.PostCategory;

public record CategoryName
{
	public const int CATEGORY_NAME_MAX_LENGTH = 50;
	
	private CategoryName() {}
	
	public required string Value { get; init; } = null!;
	
	public static Result<CategoryName> Create(string value)
	{
		if (!value.IsInRange(1, CATEGORY_NAME_MAX_LENGTH))
			return Result.Failure<CategoryName>($"Название категории должно быть от 1 до {CATEGORY_NAME_MAX_LENGTH} символов");
		
		return new CategoryName
		{
			Value = value
		};
	}
	
}