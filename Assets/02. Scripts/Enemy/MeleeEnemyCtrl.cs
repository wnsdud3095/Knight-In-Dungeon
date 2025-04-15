public class MeleeEnemyCtrl : EnemyCtrl
{
    protected override void FixedUpdate()
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

    public override void Initialize()
    {
        base.Initialize();

        m_current_hp = Script.HP;
        m_current_speed = Script.SPD;

        m_is_dead = false;

        Rigidbody.simulated = true;

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