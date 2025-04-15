using System.Collections;
using UnityEngine;

public abstract class EnemyCtrl : MonoBehaviour
{
    [field: SerializeField] public Rigidbody2D Rigidbody { get; protected set; }

    [field: SerializeField] public SpriteRenderer Renderer { get; protected set; }
    [field: SerializeField] public Animator Animator { get; protected set; }
    [field: SerializeField] public CircleCollider2D Collider { get; protected set; }

    protected Enemy m_scriptable_object;
    public Enemy Script
    {
        get { return m_scriptable_object; }
        set { m_scriptable_object = value; }
    }

    protected float m_current_hp;
    protected float m_current_speed;
    protected bool m_is_dead;

    protected Coroutine m_knockback_coroutine;
    protected Coroutine m_freeze_coroutine;

    protected abstract void FixedUpdate();

    public virtual void Initialize()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<CircleCollider2D>();
    }

    protected virtual void MoveTowardsPlayer()
    {
        Vector2 direction = GameManager.Instance.Player.transform.position - transform.position;
        if(direction == Vector2.zero)
        {
            return;
        }

        direction.Normalize();
        Rigidbody.linearVelocity = direction * m_current_speed;

        Renderer.flipX = GameManager.Instance.Player.transform.position.x < transform.position.x;
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

        Renderer.sortingOrder = 0;

        Collider.enabled = false;

        Animator.SetTrigger("Die");

        if (m_knockback_coroutine != null)
        {
            StopCoroutine(m_knockback_coroutine);
            m_knockback_coroutine = null;
        }
        if (m_freeze_coroutine != null)
        {
            StopCoroutine(m_freeze_coroutine);
            m_freeze_coroutine = null;
        }

        GameObject.Find("Stage Manager").GetComponent<StageManager>().Kill++;

        if(Script.Boss)
        {
            GameObject.Find("Stage Manager").GetComponent<SpawnManager>().BossCount--;
        }

        InstantiateExp();

        Invoke("ReturnEnemy", 2f);
    }

    protected void InstantiateExp()
    {
        GameObject exp = ObjectManager.Instance.GetObject(ObjectType.Exp);
        exp.transform.position = transform.position;

        exp.GetComponent<Exp>().SetExpAmount(Script.EXP);
    }

    protected void ReturnEnemy()
    {
        switch(Script.EnemyType)
        {
            case EnemyType.Melee:
                Destroy(GetComponent<MeleeEnemyCtrl>());
                break;
            
            case EnemyType.Ranged:
                Destroy(GetComponent<RangedEnemyCtrl>());
                break;
            
            case EnemyType.Suicide:
                Destroy(GetComponent<SuicideEnemyCtrl>());
                break;
        }

        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Enemy);
    }

    public void KnockBack(Vector2 current_position, float amount)
    {
        if(m_knockback_coroutine is not null)
        {
            StopCoroutine(m_knockback_coroutine);
        }

        m_knockback_coroutine = StartCoroutine(CoKnockBack(current_position, amount));
    }

    public IEnumerator CoKnockBack(Vector2 current_position, float amount)
    {
        Vector2 direction = -((Vector2)GameManager.Instance.Player.transform.position - current_position).normalized;

        float elasped_time = 0f;
        float target_time = 0.5f;

        Vector2 kps = direction * ((amount - Script.AntiKnockback) / target_time);

        if(kps.magnitude > 0f)
        {
            while(elasped_time <= target_time)
            {
                while (GameManager.Instance.GameState is not GameEventType.Playing)
                {
                    yield return null;
                }

                elasped_time += Time.deltaTime;
                yield return null;

                float t = elasped_time / target_time;

                Rigidbody.MovePosition(Rigidbody.position + kps * Time.deltaTime);
            }
        }

        m_knockback_coroutine = null;
    }

    public void SlowEnter(float amount)
    {
        m_current_speed *= amount + Script.AntiSlow;
    }

    public void SlowExit()
    {
        m_current_speed = Script.SPD;
    }

    public void Freeze(float duration)
    {
        if(m_freeze_coroutine is not null)
        {
            StopCoroutine(m_freeze_coroutine);
        }

        m_freeze_coroutine = StartCoroutine(CoFreeze(duration));
    }

    private IEnumerator CoFreeze(float duration)
    {
        m_current_speed = 0f;

        if(GameManager.Instance.GameState is not GameEventType.Playing)
        {
            yield return null;
        }

        yield return new WaitForSeconds(duration - Script.AntiFreeze);

        m_current_speed = Script.SPD;
    }
}