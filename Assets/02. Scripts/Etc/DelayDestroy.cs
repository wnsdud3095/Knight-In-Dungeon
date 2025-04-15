using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    [Header("파괴 지연 시간")]
    [SerializeField] private float m_delay_time;

    private void Awake()
    {
        Invoke("DestroyObject", m_delay_time);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
