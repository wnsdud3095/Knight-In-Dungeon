using System.Collections;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    [Header("몬스터의 강체")]
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }

    [Header("몬스터의 스프라이트 렌더러")]
    [field: SerializeField] public SpriteRenderer Renderer { get; private set; }

    [Header("몬스터의 애니메이터")]
    [field: SerializeField] public Animator Animator { get; private set; }

    [Header("몬스터의 콜라이더")]
    [field: SerializeField] public CircleCollider2D Collider { get; private set; }

    private Enemy m_scriptable_object;
    public Enemy Script
    {
        get { return m_scriptable_object; }
        set { m_scriptable_object = value; }
    }

    private float m_current_hp;
    private float m_current_speed;
    private bool m_is_dead;

    private Coroutine m_knockback_coroutine;
    private Coroutine m_freeze_coroutine;

    private void FixedUpdate()
    {
        if(GameManager.Instance.GameState is not GameEventType.Playing)
        {
            return;
        }

        if(m_is_dead is false && m_knockback_coroutine is null)
        {
            MoveTowardsPlayer();
        }
    }

    public void Initialize()
    {
        m_current_hp = Script.HP;
        m_current_speed = Script.SPD;

        m_is_dead = false;

        Rigidbody.simulated = true;

        Renderer.sortingOrder = 2;

        Animator.runtimeAnimatorController = Script.Animator;
        Animator.SetTrigger("Move");

        Collider.enabled = true;
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (GameManager.Instance.Player.transform.position - transform.position).normalized;

        Rigidbody.linearVelocity = direction * m_current_speed;

        Renderer.flipX = GameManager.Instance.Player.transform.position.x < transform.position.x;

        Animator.SetFloat("Speed", Rigidbody.linearVelocity.magnitude);
    }

    public void UpdateHP(float amount)
    {
        if(m_is_dead is true)
        {
            return;
        }

        m_current_hp += amount;

        if(m_current_hp <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        if(m_is_dead is true)
        {
            return;
        }

        m_is_dead = true;

        Rigidbody.linearVelocity = Vector2.zero;
        Rigidbody.simulated = false;

        Collider.enabled = false;

        Renderer.sortingOrder = 1;

        Animator.SetTrigger("Die");

        GameObject.Find("Stage Manager").GetComponent<StageManager>().Kill++;

        if(Script.Boss)
        {
            GameObject.Find("Stage Manager").GetComponent<SpawnManager>().BossCount--;
        }

        InstantiateExp();

        Invoke("ReturnEnemy", 2f);
    }

    private void InstantiateExp()
    {
        GameObject exp = ObjectManager.Instance.GetObject(ObjectType.Exp);
        exp.transform.position = transform.position;

        exp.GetComponent<Exp>().SetExpAmount(Script.EXP);
    }

    private void ReturnEnemy()
    {
        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Enemy);
    }

    public void KnockBack(Vector2 current_position, float amount)
    {
        if(m_knockback_coroutine is not null)
        {
            StopCoroutine(CoKnockBack(current_position, amount));
        }

        m_knockback_coroutine = StartCoroutine(CoKnockBack(current_position, amount));
    }

    public IEnumerator CoKnockBack(Vector2 current_position, float amount)
    {
        Vector2 direction = -((Vector2)GameManager.Instance.Player.transform.position - current_position).normalized;

        float elasped_time = 0f;
        float target_time = 0.2f;

        while(elasped_time <= target_time)
        {
            elasped_time += Time.deltaTime;
            yield return null;

            Rigidbody.MovePosition((Vector2)transform.position + direction * amount * Time.deltaTime);
        }

        transform.position += new Vector3(direction.x * amount, direction.y * amount, transform.position.z);
    }

    public void SlowEnter(float amount)
    {
        m_current_speed *= amount;
    }

    public void SlowExit()
    {
        m_current_speed = Script.SPD;
    }

    public void Freeze(float duration)
    {
        if(m_freeze_coroutine is not null)
        {
            StopCoroutine(CoFreeze(duration));
        }

        m_freeze_coroutine = StartCoroutine(CoFreeze(duration));
    }

    private IEnumerator CoFreeze(float duration)
    {
        m_current_speed = 0f;

        yield return new WaitForSeconds(duration);

        m_current_speed = Script.SPD;
    }
}
