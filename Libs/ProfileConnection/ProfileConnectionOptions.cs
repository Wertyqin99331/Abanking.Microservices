namespace ProfileConnection;

public class ProfileConnectionOptions
{
	public string Url { get; init; } = null!;
	public string ConnectionType { get; init; } = null!;
	public string? QueueServiceName { get; init; }
	public string? QueueName { get; init; }
}