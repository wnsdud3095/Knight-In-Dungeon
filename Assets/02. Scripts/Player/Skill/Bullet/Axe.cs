using UnityEngine;

public class Axe : BulletBase
{
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }

    public float Damage { get; set; }

    private void OnEnable()
    {
        Invoke("ReturnObject", 8f);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            var enemy_ctrl = collision.GetComponent<EnemyCtrl>();

            enemy_ctrl.UpdateHP(-Damage);

            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);

            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = collision.transform.position;
        }
    }

    private void ReturnObject()
    {
        gameObject.SetActive(false);
    }
}
