using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq.Expressions;
using System;

public class LD_EnemyController : MonoBehaviour
{
    private LD_EnemyFSM enemyFSM;
    [SerializeField] private EnemyData enemyData;
    public float value;     //넉백 값
    float originSpeed;      //원래 속도
    Rigidbody2D rb;
    private Vector2 moveDir;
    private Transform target;
    bool isKnockBack = false;
    public bool isFreezing { get; private set; } = false;

    void Awake()
    {
        enemyFSM = GetComponent<LD_EnemyFSM>();
    }

    void Start()
    {
        originSpeed = enemyData.moveSpeed;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // StartCoroutine(SlowRoutine());     // 슬로우 테스트
    }

    IEnumerator SlowRoutine()
    {
        GetComponent<LD_EnemyFSM>().Slow(0.5f);    // 슬로우
        yield return new WaitForSeconds(4f);    // 4초 유지
        GetComponent<LD_EnemyFSM>().ClearSlow();   // 슬로우 해제
    }

    void FixedUpdate()
    {
        if (isKnockBack) return;

        moveDir = (target.position - transform.position).normalized;
        rb.linearVelocity = moveDir * enemyFSM.currentMoveSpeed;
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어랑 닿으면 넉백 (테스트)
        // if (collision.gameObject.CompareTag("Player"))
        // {
        //     Vector2 place = collision.transform.position;
        //     StartCoroutine(KnockBackRoutine(place, value));
        // }

        // 플레이어랑 닿으면 빙결 (테스트)
        // if (collision.gameObject.CompareTag("Player"))
        // {
        //     GetComponent<LD_EnemyController>()?.Freeze(4f);
        // }
    }

    IEnumerator KnockBackRoutine(Vector2 CurrentPlace, float KnockBackValue)
    {
        isKnockBack = true;
        Vector2 dir = ((Vector2)transform.position - CurrentPlace).normalized;

        float timer = 0f;
        float knockBackTime = 0.2f;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        while (timer < knockBackTime)
        {
            rb.MovePosition(rb.position + dir * KnockBackValue * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isKnockBack = false;
    }


    public void Slow(float SlowValue)
    {
        GetComponent<LD_EnemyFSM>().currentMoveSpeed = GetComponent<LD_EnemyFSM>().GetEnemyData().moveSpeed * SlowValue;
    }

    public void ClearSlow()
    {
        GetComponent<LD_EnemyFSM>().currentMoveSpeed = GetComponent<LD_EnemyFSM>().GetEnemyData().moveSpeed;
    }

    public void Freeze(float Duration)
    {
        StartCoroutine(FreezeRoutine(Duration));
    }

    IEnumerator FreezeRoutine(float Duration)
    {
        // 빙결
        isFreezing = true;
        GetComponent<LD_EnemyFSM>().currentMoveSpeed = 0f;

        yield return new WaitForSeconds(Duration);

        // 원래 속도 복구
        isFreezing = false;
        GetComponent<LD_EnemyFSM>().currentMoveSpeed = GetComponent<LD_EnemyFSM>().GetEnemyData().moveSpeed;
    }

}

