using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Object/Create Item")]
public class Item : ScriptableObject
{
    [Header("아이템의 고유한 ID")]
    [SerializeField] private int m_id;
    public int ID
    {
        get { return m_id;}
    }

    [Header("아이템의 타입")]
    [SerializeField] private ItemType m_item_type;
    public ItemType Type
    {
        get { return m_item_type;}
    }
    
    [Header("아이템의 이미지")]
    [SerializeField] private Sprite m_item_image;
    public Sprite Image
    {
        get { return m_item_image; }
    }

    public static bool CheckEquipmentType(ItemType type)
    {
        return ItemType.HELMET <= type && type <= ItemType.SHOES;
    }
}