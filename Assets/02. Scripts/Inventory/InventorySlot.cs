using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        Item = null;
        Count = 0;
        m_item_image.sprite = null;
        SetAlpha(0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 터치된 위치에서 말풍선 생성
    }
}