namespace Core.DistributedSemaphore;

public interface IDistributedSemaphore
{
    Task<LockHandler> AcquireAsync(TimeOutValue timeOut, CancellationToken cancellationToken = default);
    Task ReleaseAsync(string nodePath);
}