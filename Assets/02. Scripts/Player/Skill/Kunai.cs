using UnityEngine;

public class Kunai : MonoBehaviour
{
    private float m_speed = 6f;
    private float m_damage;
    
    private float m_life_time = 0;
    private float m_origin_life_time = 4f;

    private int m_reflect_angle_min = 130;
    private int m_reflect_angle_max = 230;

    private int m_reflect_count = 0;

    void Update()
    {
        if(GameManager.Instance.GameState != GameEventType.Playing) return;

        transform.Translate(Vector3.up * m_speed * Time.deltaTime);

        if(m_reflect_count < 0)
        {
            ReturnToPool();
        }
        /*
        if(m_life_time > 0)
        {
            m_life_time-= Time.deltaTime;
        }
        else
        {
            ReturnToPool();
        }
        */
    }

    public void SetReflectCount(int count)
    {
        m_reflect_count = count;
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
        if (col.CompareTag("Enemy"))
        {
            //데미지 함수 호출
        }
        else if(col.CompareTag("ScreenOutLine"))
        {
            m_reflect_count--;
            transform.rotation *= Quaternion.Euler(0f, 0f, Random.Range(m_reflect_angle_min, m_reflect_angle_max));
        }
    }
}
