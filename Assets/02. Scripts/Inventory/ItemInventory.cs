using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class ItemInventory : InventoryBase
{
    public void Initialize()
    {
        Animator = GameObject.Find("Inventory UI").GetComponent<Animator>();
        Parent = GameObject.Find("Inventory Content").transform;

        if(Slots is null)
        {
            Slots = new List<InventorySlot>();
        }

        LoadSlotData();
    }

    public void AcquireItem(Item item, int count = 1, int reinforcement = 0)
    {
        for(int i = 0; i < count; i++)
        {
            InventorySlot slot = Instantiate(Prefab, Parent);
            slot.AddItem(item, count, reinforcement);

            Slots.Add(slot);
        }
    }

    public void AcquireItem(Item item, InventorySlot target_slot, int count = 1, int reinforcement = 0)
    {
        target_slot.AddItem(item, count, reinforcement);
    }

    public int GetItemCount(Item item)
    {
        int total_count = 0;

        foreach(InventorySlot slot in Slots)
        {
            if(slot.Item is null)
            {
                continue;
            }

            if(item.ID == slot.Item.ID)
            {
                total_count++;
            }
        }

        return total_count;
    }

    public void DestroySlot(int index)
    {
        Slots[index].DestroySlot();
        Slots.RemoveAt(index);
    }

    public void DestroySlot(InventorySlot slot)
    {
        slot.DestroySlot();
        Slots.Remove(slot);
    }

    public void SaveSlotData()
    {
        DataManager.Instance.Data.m_item_inventory.Clear();

        foreach(InventorySlot slot in Slots)
        {
            DataManager.Instance.Data.m_item_inventory.Add(new SlotData(slot.Item.ID, slot.Item.Type, slot.Reinforcement));
        }
    }

    public void LoadSlotData()
    {
        foreach(SlotData slot_data in DataManager.Instance.Data.m_item_inventory)
        {
            AcquireItem(ItemDataManager.Instance.GetItem(slot_data.m_item_id), 1, slot_data.m_reinforcement_level);
        }
    }
}
