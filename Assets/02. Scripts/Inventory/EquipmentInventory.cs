using UnityEngine;
using TMPro;

public class EquipmentInventory : InventoryBase
{
    [Header("현재 계산된 수치를 표현할 UI")]
    [SerializeField] private TMP_Text m_atk_label;
    [SerializeField] private TMP_Text m_hp_label;

    private EquipmentEffect m_current_equipment_effect;
    public EquipmentEffect EquipmentEffect
    {
        get { return m_current_equipment_effect; }
    }

    private void Awake()
    {
        InventorySlot[] equipment_slots = Parent.GetComponentsInChildren<InventorySlot>();
        foreach(InventorySlot slot in equipment_slots)
        {
            Slots.Add(slot);
        }
    }

    public void CalculateEffect()
    {
        EquipmentEffect calculated_effect = new EquipmentEffect();

        foreach(InventorySlot slot in Slots)
        {
            if(slot.Item is null)
            {
                continue;
            }

            calculated_effect += (slot.Item as Item_Equipment).Effect;
            calculated_effect += (slot.Item as Item_Equipment).GrowthEffect * slot.Reinforcement;
        }

        m_current_equipment_effect = calculated_effect;

        m_atk_label.text = EquipmentEffect.ATK.ToString();
        m_hp_label.text = EquipmentEffect.HP.ToString();
    }

    public InventorySlot GetEquipmentSlot(ItemType type)
    {
        switch(type)
        {
            case ItemType.HELMET:
                return Slots[0];
            
            case ItemType.ARMOR:
                return Slots[1];
            
            case ItemType.WEAPON:
                return Slots[2];
            
            case ItemType.BELT:
                return Slots[3];
            
            case ItemType.SHOES:
                return Slots[4];
        }

        return null;
    }
}
