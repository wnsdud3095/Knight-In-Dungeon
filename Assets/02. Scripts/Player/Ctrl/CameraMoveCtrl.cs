using UnityEngine;

public class CameraMoveCtrl : MonoBehaviour
{
    private float m_y_offset = 0.3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Player == null) return;

        Vector3 player_pos = GameManager.Instance.Player.transform.position;
        transform.position = new Vector3(player_pos.x,player_pos.y + m_y_offset, -10f);
    }
}
