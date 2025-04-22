using System.Collections;
using UnityEngine;

public class MagicSphere : BulletBase
{
    public float Damage { get; set; }

    private float m_target_time = 2f;
    private float m_attack_interval = 0.5f;
    private float m_attack_radius = 0.5f;
    private float m_pull_force = 1.5f;

    private float m_elapsed_time = 0f;
    private bool m_is_active = false;

    private void Update()
    {
        if (!m_is_active) 
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_attack_radius, LayerMask.GetMask("Enemy"));
        foreach (Collider2D collider in colliders)
        {
            var enemy_ctrl = collider.GetComponent<EnemyCtrl>();
            if (enemy_ctrl != null)
            {
                enemy_ctrl.transform.position = Vector2.MoveTowards(enemy_ctrl.transform.position, transform.position, m_pull_force * Time.deltaTime);
            }
        }

        m_elapsed_time += Time.deltaTime;
        if (m_elapsed_time >= m_target_time)
        {
            m_is_active = false;
        }
    }

    public void Event_SphereStart()
    {
        m_elapsed_time = 0f;
        m_is_active = true;
        StartCoroutine(AttackCoroutine());
    }

    public void Event_SphereEnd()
    {
        m_is_active = false;
        m_elapsed_time = 0f;

        gameObject.SetActive(false);
    }

    private IEnumerator AttackCoroutine()
    {
        while (m_elapsed_time < m_target_time)
        {
            yield return new WaitUntil(() => GameManager.Instance.GameState is GameEventType.Playing);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_attack_radius, LayerMask.GetMask("Enemy"));
            foreach (Collider2D collider in colliders)
            {
                var enemy_ctrl = collider.GetComponent<EnemyCtrl>();
                if (enemy_ctrl != null)
                {
                    enemy_ctrl.UpdateHP(-(Damage / 5f));

                    GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);
                    
                    damage_indicator.GetComponent<DamageIndicator>().Initialize(-(Damage / 5f));
                    damage_indicator.transform.position = transform.position;
                }
            }

            yield return new WaitForSeconds(m_attack_interval);
        }
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);
            
            damage_indicator.GetComponent<DamageIndicator>().Initialize("<size=0.125><color=red>제압</color></size>");
            damage_indicator.transform.position = col.transform.position + new Vector3(0f, -0.25f, 0f);
        }
    }    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attack_radius);
    }
}
