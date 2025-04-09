using UnityEngine;

[CreateAssetMenu(fileName = "New TimeWave", menuName = "Scriptable Object/Waves/Create Time-based Wave")]
public class TimeWave : Wave
{
    [Space(50)]
    [Header("웨이브의 지속 시간")]
    [SerializeField] float m_duration;
    public float Duration
    {
        get { return m_duration; }
    }

    [Header("스폰 대기 시간")]
    [SerializeField] float m_interval;
    public float Interval
    {
        get { return m_interval; }
    }
}
