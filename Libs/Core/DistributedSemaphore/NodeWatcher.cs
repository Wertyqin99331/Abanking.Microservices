using org.apache.zookeeper;

namespace Core.DistributedSemaphore;

public class NodeWatcher : Watcher
{
    private readonly TaskCompletionSource<bool> _taskCompletionSource = new();

    public override Task process(WatchedEvent @event)
    {
        if (@event.get_Type() == Event.EventType.NodeDeleted)
        {
            this._taskCompletionSource.SetResult(true);
        }
        return Task.CompletedTask;
    }

    public async Task WaitForNodeDeletion(string nodePath, TimeOutValue timeOut, CancellationToken cancellationToken)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(timeOut.Value);
        await this._taskCompletionSource.Task;
    }
}