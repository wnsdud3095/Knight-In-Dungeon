using UnityEngine;

public class CameraMoveCtrl : MonoBehaviour
{
    private float m_y_offset = 0.3f;

    void LateUpdate() //네트워크 오브젝트가 네트워크에서 보간되기 전에 위치에 접근해서 즉 Player가 실제 위치는 변했지만 시각적으로는 아직 반영 안된 상태에서 접근해서
                      //그렇기 때문에 모든 오브젝트 움직임이 끝난 뒤에 호출되니까 맞는 타이밍에 접근 가능
    {
        if (GameManager.Instance.Player == null) return;

        Vector3 player_pos = GameManager.Instance.Player.transform.position;
        transform.position = new Vector3(player_pos.x, player_pos.y + m_y_offset, -10f);
    }

}
