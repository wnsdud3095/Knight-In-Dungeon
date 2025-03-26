using UnityEngine;

[CreateAssetMenu(fileName = "New Gacha", menuName = "Scriptable Object/Create Gacha")]
public class Gacha : ScriptableObject
{
    [Header("가챠의 고유한 ID")]
    [SerializeField] private int m_id;
    public int ID
    {
        get { return m_id; }
    }

    [Header("가챠의 이름")]
    [SerializeField] private string m_name;
    public string Name
    {
        get { return m_name; }
    }

    [Header("가챠의 설명")]
    [SerializeField] [TextArea] private string m_description;
    public string Description
    {
        get { return m_description; }
    }

    [Header("가챠의 1회 가격")]
    [SerializeField] private int m_cost;
    public int Cost
    {
        get { return m_cost; }
    }

    [Header("가챠에서 나올 수 있는 아이템의 목록")]
    [SerializeField] private Item[] m_items;
    public Item[] Items
    {
        get { return m_items; }
    }

    [Header("가챠에서 아이템의 나올 가중치 목록")]
    [SerializeField] private int[] m_weights;
    public int[] Weights
    {
        get { return m_weights; }
    }

    [Header("가챠의 제한 레벨")]
    [SerializeField] private int m_level;
    public int Level
    {
        get { return m_level; }
    }

    [Header("가챠의 이미지")]
    [SerializeField] private Sprite m_image;
    public Sprite Image
    {
        get { return m_image; }
    }
}
