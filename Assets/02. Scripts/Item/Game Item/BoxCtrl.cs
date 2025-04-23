using UnityEngine;

public class BoxCtrl : MonoBehaviour
{
    [Header("타일 재배치 매니저")]
    [SerializeField] private TileRepositionManager m_tile_manager;

    [SerializeField] private float m_out_radius = 10f;
    [SerializeField] private float m_in_radius = 5f;
    [SerializeField] private float m_timer = 0f;

    private void Update()
    {
        m_timer += Time.deltaTime;

        if(m_timer >= 60f)
        {
            m_timer = 0f;
            CreateBox();
        }
    }

    private void CreateBox()
    {
        Vector2 spawn_position = GetValidSpawnPosition();
        GameObject box = ObjectManager.Instance.GetObject(ObjectType.Item_Box);
        box.transform.position = spawn_position;
    }

    private Vector2 GetValidSpawnPosition()
    {
        int attempt = 0;

        while(attempt < 10)
        {
            Vector2 current_spawn_pos = GetSpawnPosition();

            foreach(Transform tile in m_tile_manager.Tiles)
            {
                if(tile.GetComponent<BoxCollider2D>().OverlapPoint(current_spawn_pos))
                {
                    return current_spawn_pos;
                }
            }
            attempt++;
        }

        return Vector2.zero;        
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 center = GameManager.Instance.Player.transform.position;

        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(m_in_radius, m_out_radius);

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        Vector2 spawn_position = center + offset;

        return spawn_position;
    }
}
