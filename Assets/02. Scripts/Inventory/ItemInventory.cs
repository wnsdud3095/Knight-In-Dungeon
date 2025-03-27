using UnityEngine;

public class ItemInventory : InventoryBase
{
    public void Initialize()
    {
        Animator = GameObject.Find("Inventory UI").GetComponent<Animator>();
        Parent = GameObject.Find("Inventory Content").transform;
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

    public void AcquireItem(Item item, InventorySlot target_slot)
    {
        target_slot.AddItem(item);
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
}
