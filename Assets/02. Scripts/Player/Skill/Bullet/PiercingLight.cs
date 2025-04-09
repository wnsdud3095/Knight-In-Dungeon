using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class PiercingLight : MonoBehaviour
{
    private Animator m_animator;
    public float Damage { get; set; }

    public float LightExpand { get; set; }

    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(ExpandObject());
    }

    private IEnumerator ExpandObject()
    {
        HashSet<float> triggered_points = new HashSet<float>(); // 중복 실행 방지
        while (true)
        {
            AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
            float n_time = info.normalizedTime; // 값을 시간으로 정규

            if (n_time >= 0.98f)
            {
                ReturnToPool();
                transform.localScale = new Vector3(1, 1, 1);
                yield break;
            }
            if (n_time >= 0.25f && !triggered_points.Contains(0.25f))
            {
                transform.localScale = new Vector3(LightExpand, 1, 1);
                triggered_points.Add(0.25f);
            }
            yield return null;
        }
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //col.GetComponent<EnemyFSM>().TakeDamage(Damage);

            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);

            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = col.transform.position;
        }
    }
}
