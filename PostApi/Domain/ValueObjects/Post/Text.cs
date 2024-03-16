using Core.Extensions;
using CSharpFunctionalExtensions;

namespace Domain.ValueObjects.Post;

public record Text
{
	public const int TEXT_MIN_LENGTH = 1;
	public const int TEXT_MAX_LENGTH = 1000;
	
	private Text(string value)
	{
		this.Value = value;
	}
	
	public string Value { get; init; }

	public static Result<Text> Create(string value)
	{
		if (string.IsNullOrWhiteSpace(value) || !value.IsInRange(TEXT_MIN_LENGTH, TEXT_MAX_LENGTH))
			return Result.Failure<Text>($"Длина теста должна быть от {TEXT_MIN_LENGTH} до {TEXT_MAX_LENGTH}");

		return new Text(value);
	}
}