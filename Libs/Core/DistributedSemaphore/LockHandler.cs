namespace Core.DistributedSemaphore;

public class LockHandler(string nodePath, DistributedSemaphore semaphore) : IDisposable
{
    public string NodePath => nodePath;

    public async Task ReleaseAsync()
    {
        await semaphore.ReleaseAsync(nodePath);
    }

    public void Dispose()
    {
        ReleaseAsync().Wait();
    }
}