using UnityEngine;

public class Evolution : MonoBehaviour
{
    [Header("능력치 강화 슬롯들의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_parent;

    private EvolutionSlot[] m_slots;
    public EvolutionSlot[] Slots
    {
        get { return m_slots; }
    }

    private void Awake()
    {
        m_slots = m_slot_parent.GetComponentsInChildren<EvolutionSlot>();   
    }
}
