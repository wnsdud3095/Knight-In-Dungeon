using Fusion;
using UnityEngine;

public class SuicideEnemyCtrl : EnemyCtrl
{
    public override void FixedUpdateNetwork()
    {
        if(!HasStateAuthority)
        {
            return;
        }

        if(GameManager.Instance.GameState is not GameEventType.Playing)
        {
            return;
        }

        if(IsDead is false && m_knockback_coroutine is null)
        {
            MoveTowardsPlayer();
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        HP = Script.HP;
        SPD = Script.SPD;

        IsDead = false;

        Rigidbody.simulated = true;

        SetAniamtor();
        
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

    public void SetAniamtor()
    {
        if(HasStateAuthority)
        {
            RPC_SetAniamtor();
        }
        else
        {
            RPC_RequestSetAnimator();
        }
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SetAniamtor()
    {
        Animator.runtimeAnimatorController = Script.Animator;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_RequestSetAnimator()
    {
        RPC_SetAniamtor();
    }
    
    public void Suicide()
    {
        if(IsDead is true)
        {
            return;
        }

        IsDead = true;

        Rigidbody.linearVelocity = Vector2.zero;
        Rigidbody.simulated = false;

        Renderer.sortingOrder = 0;

        Collider.enabled = false;

        SetDieTrigger();

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

        Invoke("ReturnEnemy", 2f);
    }

    private void SetDieTrigger()
    {
        if(HasStateAuthority)
        {
            RPC_SetDieTrigger();
        }
        else
        {
            RPC_RequestSetDieTrigger();
        }
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_RequestSetDieTrigger()
    {
        RPC_SetDieTrigger();
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SetDieTrigger()
    {
        Animator.SetTrigger("Die");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Invoke("Suicide", 0.1f);
        }
    }
}
