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
}
