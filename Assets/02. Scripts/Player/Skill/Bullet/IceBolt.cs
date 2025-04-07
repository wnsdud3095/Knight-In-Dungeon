using UnityEngine;

public class IceBolt : MagicMissile
{
    void Update()
    {
        if (GameManager.Instance.GameState != GameEventType.Playing) return;

        transform.Translate(Vector3.up * Speed * Time.deltaTime);

        LifeTimeCheck();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {       
        if (col.CompareTag("Enemy"))
        {
            //col.GetComponent<EnemyFSM>().TakeDamage(Damage);

            Debug.Log($"아이스볼트 : {Damage}");
            col.GetComponent<EnemyController>().Freeze(2f);

            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);

            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = col.transform.position;
        }
    }
}
