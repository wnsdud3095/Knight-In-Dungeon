using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }

    private float m_atk;
    public float ATK
    {
        get => m_atk;
        set => m_atk = value;
    }

    private float m_speed;
    public float SPD
    {
        get => m_speed;
        set => m_speed = value;
    }

    private float m_origin_speed;
    private Vector2 m_origin_direction;
    private Coroutine m_return_coroutine;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_return_coroutine = StartCoroutine(Return());
    }

    private void OnDisable()
    {
        if(m_return_coroutine != null)
        {
            StopCoroutine(m_return_coroutine);
            m_return_coroutine = null;
        }
    }

    public void Initialize(float atk, float speed)
    {
        ATK = atk;
        SPD = speed;
        m_origin_speed = SPD;

        MoveTowardsPlayer();
        RotateTowardsDirection(m_origin_direction);
    }

    public void Stop()
    {
        SPD = 0f;
    }

    public void Resume()
    {
        SPD = m_origin_speed;

        MoveTowardsPlayer(m_origin_direction);

    }

    public void MoveTowardsPlayer()
    {
        if(!GameManager.Instance.Player)
        {
            return;
        }

        Vector2 direction = (GameManager.Instance.Player.transform.position - transform.position).normalized;
        m_origin_direction = direction;

        Rigidbody.linearVelocity = direction * SPD;
    }

    public void MoveTowardsPlayer(Vector2 direction)
    {
        if(!GameManager.Instance.Player)
        {
            return;
        }

        Rigidbody.linearVelocity = direction * SPD;
    }

    private void RotateTowardsDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private IEnumerator Return()
    {
        float elapsed_time = 0f;
        float target_time = 10f;

        while(elapsed_time <= target_time)
        {
            if(GameManager.Instance.GameState is GameEventType.Playing)
            {
                elapsed_time += Time.deltaTime;
            }

            yield return null;
        }

        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Arrow);
    }

    private void ReturnNow()
    {
        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Arrow);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Invoke("ReturnNow", 0.1f);
        }
    }
}