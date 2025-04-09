using UnityEngine;

[CreateAssetMenu(fileName = "New Boss Wave", menuName = "Scriptable Object/Waves/Create Boss Wave")]
public class BossWave : Wave
{
    [Space(50)]
    [Header("웨이브의 지속 시간")]
    [SerializeField] private float m_duration;
    public float Duration
    {
        get { return m_duration; }
    }
}
