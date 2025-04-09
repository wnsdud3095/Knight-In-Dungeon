using UnityEngine;

public class MagicMissile : MonoBehaviour
{
    private float m_damage;  

    public float Damage
    {
        get { return m_damage; }
        set { m_damage = value * m_damage_up; }
    }
    public float Speed { get; set; } = 6f;

    private int m_per_count = 1;

    protected float m_life_time = 0;
    protected float m_origin_life_time = 4f;

    protected float m_damage_up = 1f;

    protected void OnEnable()
    {
        m_life_time = m_origin_life_time;
        m_per_count = 1;
    }


    private void Update()
    {
        if (GameManager.Instance.GameState != GameEventType.Playing) return;

        transform.Translate(Vector3.up * Speed * Time.deltaTime);

        PerCheck();
        LifeTimeCheck();
    }

    private void PerCheck()
    {
        if (m_per_count <= 0) ReturnToPool();
    }

    protected void LifeTimeCheck()
    {
        

        if (m_life_time > 0)
        {
            m_life_time -= Time.deltaTime;
        }
        else
        {
            ReturnToPool();
        }
    }

    protected void ReturnToPool()
    {
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyCtrl>().UpdateHP(-Damage);
            
            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);

            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = col.transform.position;
            m_per_count--;
        }
    }
}
