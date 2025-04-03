using UnityEngine;

public class Magic_Attack : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 3f; // 화살 유지 시간
    private bool isClone = false; // 클론 여부 확인
    private Transform player; // 플레이어 위치 저장
    void Awake()
    {
        if (transform.parent == null) // 부모가 없으면 클론으로 간주
        {
            isClone = true;
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform; // 태그로 플레이어 찾기
    }

    void Start()
    {
        if (isClone)
        {
            FacePlayer(); // 플레이어를 향하게 회전
            Destroy(gameObject, lifeTime); // 클론이면 일정 시간이 지나면 삭제
        }
    }

    void FacePlayer()
    {
        if (player == null) return; // 플레이어가 없으면 회전 안 함

        Vector2 direction = (player.position - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 방향을 각도로 변환
        transform.rotation = Quaternion.Euler(0, 0, angle); // 화살 회전 적용
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isClone && other.CompareTag("Player")) // 클론이고 플레이어와 충돌 시 삭제
        {
            Destroy(gameObject);
        }
    }
}
