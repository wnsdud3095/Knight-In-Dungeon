using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    private Item m_item;
    public Item Item
    {
        get { return m_item; }
        set { m_item = value; }
    }

    private int m_count;
    public int Count
    {
        get { return m_count; }
        set { m_count = value; }
    }

    [Header("슬롯 마스크")]
    [SerializeField] private ItemType m_slot_mask;
    public ItemType SlotMask
    {
        get { return m_slot_mask; }
    }

    [Space(30)]
    [Header("UI 관련 컴포넌트")]
    public Image m_item_image;

    private ItemActionCtrl m_item_action_ctrl;

    private void Awake()
    {
        m_item_action_ctrl = GameObject.Find("Inventory Manager").GetComponent<ItemActionCtrl>();
    }

    private void SetAlpha(float alpha)
    {
        Color color = m_item_image.color;
        color.a = alpha;
        m_item_image.color = color;
    }

    public bool Mask(Item item)
    {
        return ((int)item.Type & (int)SlotMask) == 0 ? false : true;
    }

    public void AddItem(Item item, int count = 1)
    {
        Item = item;
        Count = count;
        m_item_image.sprite = item.Image;

        SetAlpha(1f);
    }

    public void UpdateSlot(int count)
    {
        Count += count;

        if(count <= 0)
        {
            DestroySlot();
        }
    }

    public void ClearSlot()
    {
        Item = null;
        Count = 0;
        m_item_image.sprite = null;
        SetAlpha(0f);
    }

    public void DestroySlot()
    {
        Destroy(gameObject);
    }

    public void UseItem()
    {
        if(Item is null)
        {
            return;
        }

        m_item_action_ctrl.UseItem(Item, this);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var tooltip = GameObject.Find("Tooltip UI").GetComponent<InventoryTooltip>();

        if(Item.CheckEquipmentType(SlotMask))
        {
            tooltip.OpenUI(Item, this, false);
        }
        else
        {
            tooltip.OpenUI(Item, this, true);
        }
        
    }
}