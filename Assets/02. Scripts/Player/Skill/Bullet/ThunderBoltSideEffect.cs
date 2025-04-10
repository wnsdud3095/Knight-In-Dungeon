using UnityEngine;

public class ThunderBoltSideEffect : BulletBase
{
    private float m_life_time = 1f;
    private float m_origin_life_time = 3f;

    private float m_cool_time = 0;
    private float m_cool_down_time = 1f;

    private void OnEnable()
    {
        m_life_time = m_origin_life_time;
    }

    protected override void Awake()
    {
        base.Awake();      
    }

    // Update is called once per frame
    void Update()
    {
        GameStateCheck();
        if (GameManager.Instance.GameState != GameEventType.Playing) return;
        LifeTimeCheck();
    }

    public void LifeTimeCheck()
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

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyCtrl>().SlowEnter(0.5f);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy")) return;
        if (m_cool_time <= m_cool_down_time)
        {
            m_cool_time += Time.deltaTime;
        }
        else
        {
            float damage = GameManager.Instance.Player.Stat.AtkDamage / 10f;
            col.GetComponent<EnemyCtrl>().UpdateHP(-damage);
            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);

            damage_indicator.GetComponent<DamageIndicator>().Initialize(damage);
            damage_indicator.transform.position = col.transform.position;
            m_cool_time = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyCtrl>().SlowExit();
        }
    }
}
