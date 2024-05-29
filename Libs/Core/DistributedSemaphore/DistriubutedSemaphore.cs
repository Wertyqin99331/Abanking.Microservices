using org.apache.zookeeper;

namespace Core.DistributedSemaphore;

public class DistributedSemaphore(string connectionString, string semaphoreRootPath) : IDistributedSemaphore
{
    private readonly ZooKeeper _zooKeeper = new(connectionString, 3000, null);

    public async Task<LockHandler> AcquireAsync(TimeOutValue timeOut, CancellationToken cancellationToken = default)
    {
        var nodePath = $"{semaphoreRootPath}/lock-";
        var nodeName = await this._zooKeeper.createAsync(
            nodePath,
            [],
            ZooDefs.Ids.OPEN_ACL_UNSAFE,
            CreateMode.EPHEMERAL_SEQUENTIAL);

        while (true)
        {
            var children = await this._zooKeeper.getChildrenAsync(semaphoreRootPath, false);
            children.Children.Sort();
            if (nodeName.EndsWith(children.Children[0]))
            {
                return new LockHandler(nodeName, this);
            }

            var previousNode = GetPreviousNode(nodeName, children.Children);
            var stat = await this._zooKeeper.existsAsync($"{semaphoreRootPath}/{previousNode}", true);
            if (stat != null)
            {
                var watcher = new NodeWatcher();
                await watcher.WaitForNodeDeletion($"{semaphoreRootPath}/{previousNode}", timeOut, cancellationToken);
            }
        }
    }

    public async Task ReleaseAsync(string nodePath)
    {
        await this._zooKeeper.deleteAsync(nodePath);
    }

    private static string? GetPreviousNode(string currentNode, System.Collections.Generic.List<string> nodes)
    {
        var currentIndex = nodes.IndexOf(currentNode[(currentNode.LastIndexOf('/') + 1)..]);
        return currentIndex > 0 ? nodes[currentIndex - 1] : null;
    }
}