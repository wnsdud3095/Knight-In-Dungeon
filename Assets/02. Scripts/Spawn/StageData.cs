using UnityEngine;

[CreateAssetMenu(fileName = "New Stage Data", menuName = "Scriptable Object/Create Stage Data")]
public class StageData : ScriptableObject
{
    [SerializeField] private WaveData[] m_waves;
    public WaveData[] Waves
    {
        get { return m_waves; }
    }
}
