using Fusion;
using UnityEngine;

public class BoxCtrl : NetworkBehaviour
{
    private NetworkObjectManager m_object_pool_manager;

    [SerializeField] private float m_out_radius = 10f;
    [SerializeField] private float m_in_radius = 5f;

    [Networked] [SerializeField] public float Timer { get; private set; }

    public override void Spawned()
    {
        m_object_pool_manager = FindFirstObjectByType<NetworkObjectManager>();
    }

    public override void FixedUpdateNetwork()
    {
        if(!HasStateAuthority)
        {
            return;
        }

        Timer += GameManager.Instance.NowRunner.DeltaTime;

        if(Timer >= 60f)
        {
            Timer = 0f;
            CreateBox();
        }
    }

    private void CreateBox()
    {
        Vector2 center = GameManager.Instance.Player.transform.position;

        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(m_in_radius, m_out_radius);

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        Vector2 spawn_position = center + offset;

        GameManager.Instance.NowRunner.Spawn(m_object_pool_manager.GetPrefab(ObjectType.Item_Box), spawn_position);
    }
}
