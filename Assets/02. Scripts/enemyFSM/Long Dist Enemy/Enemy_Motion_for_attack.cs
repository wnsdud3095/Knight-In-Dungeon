using UnityEngine;
using System.Collections;

public class Enemy_Motion_for_Attack : MonoBehaviour
{
    public float speed = 2f; // 이동 속도
    public float attackRange = 3f; // 공격 범위
    public float attackCooldown = 2f; // 공격 쿨타임
    public GameObject arrowPrefab; // 화살 프리팹
    public Transform firePoint; // 화살 발사 위치

    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Animator anim;
    private Vector2 dirVec;
    private bool isLive = true;
    private bool canAttack = true;
    private Transform player;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (!isLive || player == null) return;

        float distance = Vector2.Distance(player.position, rigid.position);

        if (distance > attackRange)
        {
            // 플레이어가 공격 범위 밖이면 이동
            dirVec = (Vector2)player.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.linearVelocity = Vector2.zero;
        }
        else
        {
            // 플레이어가 공격 범위 안에 있으면 멈추고 공격
            rigid.linearVelocity = Vector2.zero;
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    void LateUpdate()
    {
        if (!isLive || player == null) return;

        anim.SetFloat("Speed", dirVec.magnitude);
        spriter.flipX = player.position.x < rigid.position.x;
    }

    IEnumerator Attack()
    {
        canAttack = false;
        anim.SetTrigger("Attack"); // 공격 애니메이션 실행

        yield return new WaitForSeconds(0.5f); // 애니메이션 딜레이 (필요하면 조정)

        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();

            if (arrowRb != null)
            {
                Vector2 direction = (player.position - firePoint.position).normalized;
                arrowRb.linearVelocity = direction * 10f; // 화살 속도 조정
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
