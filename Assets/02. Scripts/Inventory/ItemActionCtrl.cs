using UnityEngine;

public class ItemActionCtrl : MonoBehaviour
{
    private ItemInventory m_item_inventory;
    private EquipmentInventory m_equipment_inventory;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_item_inventory = GetComponent<ItemInventory>();
        m_equipment_inventory = GetComponent<EquipmentInventory>();
    }

    public void UseItem(Item item, InventorySlot called_slot = null)
    {
        switch(item.Type)
        {
            case ItemType.HELMET:
            case ItemType.ARMOR:
            case ItemType.WEAPON:
            case ItemType.BELT:
            case ItemType.SHOES:
            {
                if(Item.CheckEquipmentType(called_slot.SlotMask))
                {
                    m_item_inventory.AcquireItem(item, 1, called_slot.Reinforcement);
                    called_slot.ClearSlot();
                }
                else
                {
                    InventorySlot equipment_slot = m_equipment_inventory.GetEquipmentSlot(item.Type);

                    Item temp_item = equipment_slot.Item;
                    int temp_reinforcement = equipment_slot.Reinforcement;

                    equipment_slot.AddItem(item, 1, called_slot.Reinforcement);

                    if(temp_item is null)
                    {
                        m_item_inventory.DestroySlot(called_slot);
                    }
                    else
                    {
                        called_slot.AddItem(temp_item, 1, temp_reinforcement);
                    }
                }

                m_equipment_inventory.CalculateEffect();

                // TODO: 장비 착용 VFX 재생

                break;
            }
        }
    }
}
