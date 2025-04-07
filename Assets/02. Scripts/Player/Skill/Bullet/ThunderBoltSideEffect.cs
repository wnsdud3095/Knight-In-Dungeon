using UnityEngine;

public class ThunderBoltSideEffect : MonoBehaviour
{
    private float m_life_time = 1f;
    private float m_origin_life_time = 3f;

    private float m_damage;

    private void OnEnable()
    {
        m_life_time = m_origin_life_time;
    }

    private void Awake()
    {
        m_damage = GameManager.Instance.Player.Stat.AtkDamage / 10f;
    }

    // Update is called once per frame
    void Update()
    {
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
            Debug.Log($"{collision.gameObject.name} 둔화");
            //둔화 메서드
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"{collision.gameObject.name} 둔화 끝");
            //둔화 해제 메서드 
        }
    }
}
