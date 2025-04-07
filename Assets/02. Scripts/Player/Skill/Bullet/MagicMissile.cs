using UnityEngine;

public class MagicMissile : MonoBehaviour
{
    public float Damage { get; set; }
    public float Speed { get; set; } = 6f;

    private int m_per_count = 1;

    private float m_life_time = 0;
    private float m_origin_life_time = 4f;


    private void OnEnable()
    {
        m_life_time = m_origin_life_time;
        m_per_count = 1;
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState != GameEventType.Playing) return;

        transform.Translate(Vector3.up * Speed * Time.deltaTime);

        LifeTimeCheck();
    }

    public void LifeTimeCheck()
    {
        if(m_per_count<=0) ReturnToPool();

        if (m_life_time > 0)
        {
            m_life_time -= Time.deltaTime;
        }
        else
        {
            ReturnToPool();
        }
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        /*
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyFSM>().TakeDamage(Damage);
            
            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);

            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = col.transform.position;
            m_per_count--;
        }*/
    }
}
