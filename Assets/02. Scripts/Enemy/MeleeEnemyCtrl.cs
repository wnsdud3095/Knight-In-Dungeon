using Fusion;
using UnityEngine;

public class MeleeEnemyCtrl : EnemyCtrl
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
}