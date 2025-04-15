using UnityEngine;

public class RepositionCtrl : MonoBehaviour
{
    private float lastMoveTime = -999f;
    private float moveCooldown = 0.1f;

    private int m_tile_size = 20;

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("GroundCheckArea")) return;

        if (Time.time - lastMoveTime < moveCooldown) return;

        lastMoveTime = Time.time;


        Vector3 player_pos = GameManager.Instance.Player.transform.position;
        Vector3 my_pos = transform.position;

        Debug.Log($"[{name}] Exit! Player at {player_pos}, Me at {my_pos}");

        float dir_x = player_pos.x - my_pos.x;
        float dir_y = player_pos.y- my_pos.y;

        float diff_x = Mathf.Abs(dir_x);
        float diff_y = Mathf.Abs(dir_y);

        dir_x = dir_x > 0 ? 1 : -1;
        dir_y = dir_y > 0 ? 1 : -1;

        switch(transform.tag)
        {
            case "Ground":
                if (Mathf.Abs(diff_x - diff_y) < 0.5f)
                {
                    // 두 방향으로 이동 (대각선)
                    transform.Translate(Vector3.right * dir_x * m_tile_size * 2);
                    transform.Translate(Vector3.up * dir_y * m_tile_size * 2);
                }
                else if (diff_x > diff_y)
                {
                    transform.Translate(Vector3.right * dir_x * m_tile_size * 2);
                }
                else //if (diff_x < diff_y)
                {
                    transform.Translate(Vector3.up * dir_y * m_tile_size * 2);
                }
                
                break;
        }
    }
}
