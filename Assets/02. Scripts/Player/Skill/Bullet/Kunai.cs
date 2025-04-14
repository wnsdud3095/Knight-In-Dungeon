using UnityEngine;

public class Kunai : BulletBase
{ 
    public float Damage { get; set; }
    public float ReflectCount { get; set; }

    [HideInInspector]
    protected float m_life_time = 0;
    private float m_origin_life_time = 6f;

    private float m_speed = 6f;

    private void OnEnable()
    {
        m_life_time = m_origin_life_time;
    }

    void Update()
    {
        GameStateCheck();
        if (GameManager.Instance.GameState != GameEventType.Playing) return;

        transform.Translate(Vector3.up * m_speed * Time.deltaTime);

        if(ReflectCount < 0)
        {
            ReturnToPool();
        }

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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyCtrl>().UpdateHP(-Damage);

            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);
            
            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = col.transform.position;
        }
        else if(col.CompareTag("ScreenOutLine"))
        {
            ReflectCount--;
            Vector3 dir = GameManager.Instance.Player.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        }
    }
}
