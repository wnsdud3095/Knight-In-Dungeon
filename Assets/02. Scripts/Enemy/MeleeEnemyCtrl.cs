using UnityEngine;

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
}