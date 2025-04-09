using UnityEngine;

[CreateAssetMenu(fileName = "New Wave Data", menuName = "Scriptable Object/Create Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("웨이브 패턴")]
    [SerializeField] Wave m_pattern;
    public Wave Pattern
    {
        get { return m_pattern; }
    }

    [Header("웨이브에서 스폰할 몬스터의 종류")]
    [SerializeField] private Enemy[] m_enemies;
    public Enemy[] Enemies
    {
        get { return m_enemies; }
    }
}
