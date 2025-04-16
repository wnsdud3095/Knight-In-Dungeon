using Fusion;
using System.Collections.Generic;

public class Test : NetworkObjectProviderDefault
{
    private readonly Dictionary<NetworkPrefabId, Queue<NetworkObject>> _pool = new();

    public override NetworkObjectAcquireResult AcquirePrefabInstance(NetworkRunner runner, in NetworkPrefabAcquireContext context, out NetworkObject instance)
    {
        var prefabId = context.PrefabId;

        if (_pool.TryGetValue(prefabId, out var queue) && queue.Count > 0)
        {
            instance = queue.Dequeue();
            instance.gameObject.SetActive(true);
        }
        else
        {
            var prefab = runner.Prefabs.Load(prefabId, isSynchronous: context.IsSynchronous);
            if (!prefab)
            {
                instance = null;
                return NetworkObjectAcquireResult.Retry;
            }

            instance = UnityEngine.Object.Instantiate(prefab);
        }

        if (context.DontDestroyOnLoad)
            runner.MakeDontDestroyOnLoad(instance.gameObject);
        else
            runner.MoveToRunnerScene(instance.gameObject);

        runner.Prefabs.AddInstance(prefabId);
        return NetworkObjectAcquireResult.Success;
    }

    public override void ReleaseInstance(NetworkRunner runner, in NetworkObjectReleaseContext context)
    {
        var prefabId = context.TypeId.AsPrefabId;

        if (!_pool.TryGetValue(prefabId, out var queue))
            queue = _pool[prefabId] = new Queue<NetworkObject>();

        context.Object.gameObject.SetActive(false);
        queue.Enqueue(context.Object);

        runner.Prefabs.RemoveInstance(prefabId);
    }
}
