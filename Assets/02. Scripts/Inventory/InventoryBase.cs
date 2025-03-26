using System.Collections.Generic;
using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    [Header("인벤토리 애니메이터")]
    [SerializeField] private Animator m_animator;
    protected Animator Animator
    {
        get { return m_animator; }
        set { m_animator = value; }
    }

    [Header("인벤토리 슬롯 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_parent;
    protected Transform Parent
    {
        get { return m_slot_parent; }
        set { m_slot_parent = value; }
    }

    [Header("인벤토리 슬롯 프리펩")]
    [SerializeField] private InventorySlot m_slot_prefab;
    protected InventorySlot Prefab
    {
        get { return m_slot_prefab; }
        set { m_slot_prefab = value; }
    }

    [Header("인벤토리 슬롯 리스트")]
    private List<InventorySlot> m_slots;
    public List<InventorySlot> Slots
    {
        get { return m_slots; }
    }

    protected void Awake()
    {
        m_slots = new List<InventorySlot>();
    }
}
