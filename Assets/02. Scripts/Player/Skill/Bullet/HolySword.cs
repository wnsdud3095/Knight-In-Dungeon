using UnityEngine;

public class HolySword : BulletBase
{
    public float Damage { get; set; }

    [SerializeField] private BoxCollider2D[] m_colliders;

    private void Update()
    {
        GameStateCheck();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            var enemy_ctrl = collision.GetComponent<EnemyCtrl>();
            
            enemy_ctrl.UpdateHP(-Damage);
            enemy_ctrl.KnockBack(transform.position, 0.2f);

            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);
            
            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = collision.transform.position;            
        }
    }

    public void Event_SwordStart()
    {
        m_colliders[0].enabled = true;
        m_colliders[1].enabled = false;
    }

    public void Event_SwordIng()
    {
        m_colliders[0].enabled = false;
        m_colliders[1].enabled = true;
    }

    public void Event_SwordEnd()
    {
        m_colliders[0].enabled = false;
        m_colliders[1].enabled = false;

        gameObject.SetActive(false);
    }
}
