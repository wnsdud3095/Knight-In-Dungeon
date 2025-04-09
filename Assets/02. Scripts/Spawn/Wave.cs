using UnityEngine;

public abstract class Wave : ScriptableObject
{
    [Header("웨이브에서 스폰될 몬스터의 종류")]
    [SerializeField] private WaveSpawnType m_spawn_type;
    public WaveSpawnType SpawnType
    {
        get { return m_spawn_type; }
    }

    [Header("웨이브의 생성 패턴")]
    [SerializeField] private SpawnPattern m_spawn_pattern;
    public SpawnPattern Pattern
    {
        get { return m_spawn_pattern; }
    }

    [Header("웨이브 종료 방식")]
    [SerializeField] private WaveEndType m_end_type;
    public WaveEndType EndType
    {
        get { return m_end_type; }
    }
}