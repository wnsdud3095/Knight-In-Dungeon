using UnityEngine;

public class BulletBase : MonoBehaviour
{
    protected Animator m_animator;

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    protected void GameStateCheck()
    {
        if(GameManager.Instance.GameState != GameEventType.Playing)
        {
            m_animator.speed = 0;
        }
        else
        {
            m_animator.speed = 1f;
        }
    }
}
