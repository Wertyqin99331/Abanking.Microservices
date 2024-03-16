using Core.Extensions;
using CSharpFunctionalExtensions;

namespace Domain.ValueObjects.Post;

public record Title
{
	public const int TITLE_MIN_LENGTH = 1;
	public const int TITLE_MAX_LENGTH = 50;
	
	private Title(string value)
	{
		this.Value = value;
	}
	
	public string Value { get; init; }

	public static Result<Title> Create(string value)
	{
		if (string.IsNullOrWhiteSpace(value) || !value.IsInRange(TITLE_MIN_LENGTH, TITLE_MAX_LENGTH))
			return Result.Failure<Title>($"Длина заголовка должна быть от {TITLE_MIN_LENGTH} до {TITLE_MAX_LENGTH}");

		return new Title(value);
	}
}