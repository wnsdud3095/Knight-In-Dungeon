using UnityEngine;

public class Shuriken : Kunai
{
    private void Update()
    {
        GameStateCheck();
        //if (GameManager.Instance.GameState != GameEventType.Playing) return;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyCtrl enemy = col.GetComponent<EnemyCtrl>();
            enemy.UpdateHP(-Damage);
            enemy.KnockBack(transform.position, 0.2f);

            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);
            
            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = col.transform.position;
        }
    }
}