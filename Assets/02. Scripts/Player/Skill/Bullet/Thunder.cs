using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    protected Animator m_animator;

    public float Damage { get; set; }

    [SerializeField]
    protected BoxCollider2D m_col;


    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    protected void OnEnable()
    {
        StartCoroutine(EnableCollider());
    }

    protected virtual IEnumerator EnableCollider()
    {
        HashSet<float> triggered_points = new HashSet<float>(); // 중복 실행 방지

        while (true)
        {
            //현재 실행중인 애니메이션의 정보 불러옴
            AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
            float n_time = info.normalizedTime; // 값을 시간으로 정규

            if (n_time >= 0.99f)
            {
                m_col.enabled = false;
                transform.gameObject.SetActive(false);
                yield break; // 코루틴 종료
            }

            if (n_time >= 0.85f && !triggered_points.Contains(0.85f))
            {
                m_col.enabled = true;
                triggered_points.Add(0.85f);
            }

            yield return null; // 다음 프레임까지 대기
        }
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyFSM>().TakeDamage(Damage);

            Debug.Log("적중");
        }
    }
}
