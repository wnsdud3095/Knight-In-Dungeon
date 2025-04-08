using UnityEngine;

public class Shuriken : Kunai
{
    public float LifeTime { get {return m_life_time; }  set { m_life_time =value; } }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameEventType.Playing) return;
        LifeTimeCheck();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyFSM>().TakeDamage(Damage);
            EnemyController e_ctrl = col.GetComponent<EnemyController>();

            e_ctrl.StartCoroutine(e_ctrl.KnockBackRoutine(transform.position, 5f));

            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);
            
            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = col.transform.position;

            
        }
    }

}
