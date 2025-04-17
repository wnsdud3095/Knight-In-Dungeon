using System.Collections.Generic;
using Fusion;
using UnityEngine;

[System.Serializable]
public class NetworkPool
{
    public ObjectType m_object_type;
    public int m_max_size;
    public Transform m_container;
    public GameObject m_prefab;
    public NetworkPrefabId m_prefab_id;
    public Queue<NetworkObject> m_pool = new();
}

public class NetworkObjectManager : NetworkObjectProviderDefault
{
    [Header("네트워크 풀 목록")]
    [SerializeField] private List<NetworkPool> m_pool_list;

    private Dictionary<ObjectType, GameObject> m_prefab_dict = new();

    private Dictionary<NetworkPrefabId, NetworkPool> m_object_pool = new();
    
    private void Awake()
    {
        foreach(NetworkPool pool in m_pool_list)
        {
            m_prefab_dict[pool.m_object_type] = pool.m_prefab;
            m_object_pool[pool.m_prefab_id] = pool;
        }
    }

    public override NetworkObjectAcquireResult AcquirePrefabInstance(NetworkRunner runner, in NetworkPrefabAcquireContext context, out NetworkObject instance)
    {
        var prefab_id = context.PrefabId;

        if(m_object_pool.TryGetValue(prefab_id, out var network_pool) && network_pool.m_pool.Count > 0)
        {
            instance = network_pool.m_pool.Dequeue();
            instance.gameObject.SetActive(true);
        }
        else
        {
            var prefab = runner.Prefabs.Load(prefab_id, context.IsSynchronous);
            if(!prefab)
            {
                instance = null;
                return NetworkObjectAcquireResult.Retry;
            }

            instance = Instantiate(prefab);

            if(network_pool is not null)
            {
                instance.transform.SetParent(network_pool.m_container);
            }
        }

        if(context.DontDestroyOnLoad)
        {
            runner.MakeDontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            runner.MoveToRunnerScene(instance.gameObject);
        }

        runner.Prefabs.AddInstance(prefab_id);

        return NetworkObjectAcquireResult.Success;
    }

    public override void ReleaseInstance(NetworkRunner runner, in NetworkObjectReleaseContext context)
    {
        var prefab_id = context.TypeId.AsPrefabId;

        if (!m_object_pool.TryGetValue(prefab_id, out var network_pool))
        {
            Destroy(context.Object.gameObject);
        }
        else if(network_pool.m_pool.Count < network_pool.m_max_size)
        {
            context.Object.gameObject.SetActive(false);
            network_pool.m_pool.Enqueue(context.Object);
        }
        else
        {
            Destroy(context.Object.gameObject);
        }

        runner.Prefabs.RemoveInstance(prefab_id);
    }

    public GameObject GetPrefab(ObjectType type)
    {
        return m_prefab_dict.TryGetValue(type, out var prefab) ? prefab : null;
    }
}
