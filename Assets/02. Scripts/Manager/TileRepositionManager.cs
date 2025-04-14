using UnityEngine;
using System.Collections.Generic;

public class TileRepositionManager : MonoBehaviour
{
    public GameObject m_tile_prefab;
    private Transform m_player;
    public int m_tile_size = 20;
    private List<Transform> m_tiles = new List<Transform> ();
    private Vector2Int m_last_center;

    void Start()
    {
        m_player = GameManager.Instance.Player.transform;
        
        for (int y = -1; y <= 1; y++)// 초기 3x3 타일 생성
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3 pos = new Vector3(x * m_tile_size, y * m_tile_size, 0);
                GameObject tile = Instantiate(m_tile_prefab, pos, Quaternion.identity, transform);
                m_tiles.Add(tile.transform);
            }
        }

        m_last_center = GetPlayerTileIndex();
    }

    void Update()
    {
        Vector2Int current_center = GetPlayerTileIndex(); // 플레이어의 현재 위치 불러옴

        if(current_center != m_last_center) // 현재 플레이어가 있는 타일 위치와 다르면 타일 재배치
        {
            RepositionTiles(current_center);
            m_last_center=current_center;
        }
    }

    private Vector2Int GetPlayerTileIndex()
    {
        return new Vector2Int(
            Mathf.FloorToInt(m_player.position.x / m_tile_size),
            Mathf.FloorToInt(m_player.position.y / m_tile_size)
        );
    }
    private void RepositionTiles(Vector2Int center)
    {
        int i = 0;
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector2Int offset = new Vector2Int(x, y);
                Vector3 new_pos = new Vector3((center.x + offset.x) * m_tile_size,(center.y + offset.y) * m_tile_size,0);
                m_tiles[i].position = new_pos;
                i++;
            }
        }
    }
}
