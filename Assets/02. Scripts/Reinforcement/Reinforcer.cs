using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Reinforcer : MonoBehaviour
{
    [Header("아이템 툴팁 UI의 애니메이터")]
    [SerializeField] private InventoryTooltip m_tooltip_ui_object;

    [Space(50)] [Header("강화 UI의 컴포넌트")]
    [Header("강화 UI의 애니메이터")]
    [SerializeField] private Animator m_reinforcement_ui_object;

    [Header("강화된 아이템이 위치할 슬롯")]
    [SerializeField] private InventorySlot m_target_slot;

    [Header("재료 아이템들이 위치할 슬롯의 부모")]
    [SerializeField] private Transform m_ingredient_slot_parent;
    private InventorySlot[] m_ingredient_slots;

    [Header("강화 버튼")]
    [SerializeField] private Button m_reinforce_button;

    private ItemInventory m_item_inventory;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_tooltip_ui_object = GameObject.Find("Tooltip UI").GetComponent<InventoryTooltip>();
        m_reinforcement_ui_object = GameObject.Find("Reinforcement UI").GetComponent<Animator>();
        m_item_inventory = GameObject.Find("Inventory Manager").GetComponent<ItemInventory>();

        m_ingredient_slots = m_ingredient_slot_parent.GetComponentsInChildren<InventorySlot>();
    }

    public void OpenUI(InventorySlot current_slot, Item item)
    {
        if(m_item_inventory is null)
        {
            return;
        }

        m_tooltip_ui_object.Button_CloseUI();
        m_reinforcement_ui_object.SetBool("Open", true);

        UpdateIngredientSlot(item);

        if(CheckCanReinforcement())
        {
            m_reinforce_button.interactable = CheckCanReinforcement();
            m_target_slot.AddItem(item, 1, GetNextReinforcement());
        }
    }

    public void UpdateIngredientSlot(Item item)
    {
        List<InventorySlot> same_item_slots = new List<InventorySlot>();
        foreach(InventorySlot slot in m_item_inventory.Slots)
        {
            if(slot.Item.ID == item.ID)
            {
                same_item_slots.Add(slot);
            }
        }

        same_item_slots.Sort(delegate (InventorySlot arg1, InventorySlot arg2) 
                                    { 
                                        return arg2.Reinforcement.CompareTo(arg1.Reinforcement); 
                                    });

        if(same_item_slots.Count >= 3)
        {
            for(int i = 0; i < 3; i++)
            {
                m_ingredient_slots[i].AddItem(same_item_slots[i].Item, 1, same_item_slots[i].Reinforcement);
            }
        }
        else
        {
            for(int i = 0; i < same_item_slots.Count; i++)
            {
                m_ingredient_slots[i].AddItem(same_item_slots[i].Item, 1, same_item_slots[i].Reinforcement);
            }
        }
    }

    private bool CheckCanReinforcement()
    {
        foreach(InventorySlot slot in m_ingredient_slots)
        {
            if(slot.Item is null)
            {
                return false;
            }
        }

        return true;
    }

    private int GetNextReinforcement()
    {
        return m_ingredient_slots[0].Reinforcement + 1;
    }

    public void Button_CloseUI()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        for(int i = 0; i < 3; i++)
        {
            m_ingredient_slots[i].ClearSlot();
        }

        m_tooltip_ui_object.GetComponent<Animator>().SetBool("Open", true);
        m_reinforcement_ui_object.SetBool("Open", false);
    }

    public void CloseUI()
    {
        for(int i = 0; i < 3; i++)
        {
            m_ingredient_slots[i].ClearSlot();
        }

        for(int i = 0; i < m_item_inventory.Slots.Count; i++)
        {
            if(m_item_inventory.Slots[i].Item.ID == m_target_slot.Item.ID)
            {
                if(m_item_inventory.Slots[i].Reinforcement == GetNextReinforcement())
                {
                    m_tooltip_ui_object.OpenUI(m_item_inventory.Slots[i].Item, m_item_inventory.Slots[i]);
                    break;
                }
            }
            
        }

        m_reinforcement_ui_object.SetBool("Open", false);        
    }

    public void Button_Reinforcement()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        m_item_inventory.AcquireItem(m_target_slot.Item, 1, GetNextReinforcement());

        for(int index = 2; index >= 0; index--)
        {
            for(int i = m_item_inventory.Slots.Count - 1; i >= 0; i--)
            {
                if(m_item_inventory.Slots[i].Item.ID == m_ingredient_slots[index].Item.ID &&
                   m_item_inventory.Slots[i].Reinforcement == m_ingredient_slots[index].Reinforcement)
                {
                    m_item_inventory.DestroySlot(i);
                    break;
                }
            }
        }

        CloseUI();

        m_target_slot.ClearSlot();
    }
}
