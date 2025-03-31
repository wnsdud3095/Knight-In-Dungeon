using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Scriptable Object/Create Equipment Item")]
public class Item_Equipment : Item
{
    [Space(50)] [Header("장비 아이템 효과 (착용 시)")]
    [SerializeField] private EquipmentEffect m_effect;
    public EquipmentEffect Effect
    {
        get { return m_effect; }
    }

    [Header("1회 강화 시, 증가할 효과")]
    [SerializeField] private EquipmentEffect m_growth_effect;
    public EquipmentEffect GrowthEffect
    {
        get { return m_growth_effect; }
    }

    [Header("최대 강화치")]
    [SerializeField] private int m_reinforcement_limit;
    public int MaxReinforce
    {
        get { return m_reinforcement_limit; }
    }
}
