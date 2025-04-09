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
            EnemyCtrl enemy = col.GetComponent<EnemyCtrl>();
            enemy.UpdateHP(-Damage);
            enemy.Knockback(transform.position, 5f);

            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);
            
            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = col.transform.position;
        }
    }
}