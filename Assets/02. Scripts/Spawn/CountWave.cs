using UnityEngine;

[CreateAssetMenu(fileName = "New CountWave", menuName = "Scriptable Object/Waves/Create Count-based Wave")]
public class CountWave : Wave
{
    [Space(50)]
    [Header("생성할 몬스터의 수")]
    [SerializeField] private int m_count;
    public int Count
    {
        get { return m_count; }
    }
}
