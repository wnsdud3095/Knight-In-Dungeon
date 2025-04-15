using UnityEngine;

public class SuicideEnemyCtrl : MeleeEnemyCtrl
{
    public void Suicide()
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

        Invoke("ReturnEnemy", 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Invoke("Suicide", 0.1f);
        }
    }
}
