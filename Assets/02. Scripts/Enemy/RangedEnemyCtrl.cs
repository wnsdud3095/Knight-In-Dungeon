using UnityEngine;

public class RangedEnemyCtrl : EnemyCtrl
{
    private float m_atk_elapsed_interval = 2f;
    private float m_atk_target_interval = 2f;

    protected override void FixedUpdate()
    {
        if(m_is_dead)
        {
            return;
        }

        if(GameManager.Instance.GameState is not GameEventType.Playing)
        {
            return;
        }

        if(m_is_dead is false && m_knockback_coroutine is null)
        {
            float distance = Vector2.Distance(GameManager.Instance.Player.transform.position, transform.position);

            if(distance > (Script as RangedEnemy).Range)
            {
                MoveTowardsPlayer();
            }
            else
            {
                TryAttackPlayer();
            }
        }
    }

    public void Update()
    {
        if(GameManager.Instance.GameState is GameEventType.Playing)
        {
            m_atk_elapsed_interval += Time.deltaTime;
            m_atk_elapsed_interval = Mathf.Clamp(m_atk_elapsed_interval, 0f, m_atk_target_interval);
        }
    }

    protected override void MoveTowardsPlayer()
    {
        Vector2 direction = GameManager.Instance.Player.transform.position - transform.position;
        if(direction == Vector2.zero)
        {
            return;
        }

        Animator.SetBool("IsMove", true);

        direction.Normalize();
        Rigidbody.linearVelocity = direction * m_current_speed;

        Renderer.flipX = GameManager.Instance.Player.transform.position.x < transform.position.x;
    }

    public void TryAttackPlayer()
    {
        Animator.SetBool("IsMove", false);

        Rigidbody.linearVelocity = Vector2.zero;
        Renderer.flipX = GameManager.Instance.Player.transform.position.x < transform.position.x;

        if(m_atk_elapsed_interval >= m_atk_target_interval)
        {
            Animator.SetTrigger("Attack");

            GameObject arrow_object = ObjectManager.Instance.GetObject(ObjectType.Arrow);
            arrow_object.transform.position = transform.position;
            
            Arrow arrow = arrow_object.GetComponent<Arrow>();

            arrow.Initialize(Script.ATK, (Script as RangedEnemy).ProjectileSPD);

            m_atk_elapsed_interval = 0f;
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        m_current_hp = Script.HP;
        m_current_speed = Script.SPD;

        m_is_dead = false;

        Rigidbody.simulated = true;
        Rigidbody.linearVelocity = Vector2.zero;

        Animator.runtimeAnimatorController = Script.Animator;
        Animator.ResetTrigger("Die");

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

        Collider.enabled = true;
    }
}
