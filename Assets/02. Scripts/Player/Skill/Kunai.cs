using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 0.4f;
    private float m_damage;
    
    private float m_life_time = 0;
    private float m_origin_life_time = 4f;

    void Start()
    {

    }

    void Update()
    {
        if(GameManager.Instance.GameState != GameEventType.Playing) return;

        transform.Translate(Vector3.up * m_speed * Time.deltaTime);

        if(m_life_time > 0)
        {
            m_life_time-= Time.deltaTime;
        }
        else
        {
            ReturnToPool();
        }
    }

    public void SetDamage(float damage)
    {
        m_damage= damage;
    }

    public void SetLifeTime()
    {
        m_life_time = m_origin_life_time;
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {
            //데미지 함수 호출
        }
    }
}
