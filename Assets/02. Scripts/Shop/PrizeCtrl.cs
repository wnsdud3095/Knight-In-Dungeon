using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PrizeCtrl : MonoBehaviour
{
    [SerializeField] private ItemInventory m_item_inventory;

    [Space(30)]
    [Header("보상 패널의 애니메이터")]
    [SerializeField] private Animator m_prize_ui_object;

    [Header("인벤토리 슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_parent;

    [Header("인벤토리 슬롯의 트랜스폼")]
    [SerializeField] private InventorySlot m_slot_prefab;

    [Header("종료 버튼")]
    [SerializeField] private Button m_exit_button;

    private List<InventorySlot> m_slots = new List<InventorySlot>();

    private int m_total_weight;

    private Transform m_inventory_slot_container;

    private void Awake()
    {
        m_inventory_slot_container = GameObject.Find("Inventory Slot Container").transform;
    }

    public void OpenUI(Gacha gacha, int count)
    {
        m_prize_ui_object.SetBool("Open", true);

        m_total_weight = 0;
        foreach(int weight in gacha.Weights)
        {
            m_total_weight += weight;
        }

        List<Item> m_prize_item = new List<Item>();

        StartCoroutine(ShowCoroutine(gacha, count));
    }

    public void Button_CloseUI()
    {
        SoundManager.Instance.PlayEffect("Button Click");
        
        foreach(var slot in m_slots)
        {
            slot.transform.SetParent(m_inventory_slot_container);
            ObjectManager.Instance.ReturnObject(slot.gameObject, ObjectType.InventorySlot);
        }
        m_slots.Clear();
        
        m_prize_ui_object.SetBool("Open", false);
    }

    private IEnumerator ShowCoroutine(Gacha gacha, int count)
    {
        m_exit_button.enabled = false;
        yield return new WaitForSeconds(0.5f);

        for(int j = 0; j < count; j++)
        {
            int random = Random.Range(0, m_total_weight);
            int accumulated_weight = 0;
            Item selected_item = null;

            for(int i = 0; i < gacha.Items.Length; i++)
            {
                accumulated_weight += gacha.Weights[i];
                if(random < accumulated_weight)
                {
                    selected_item = gacha.Items[i];
                    break;
                }
            }

            if(selected_item != null)
            {
                m_item_inventory.AcquireItem(selected_item);

                InventorySlot slot = ObjectManager.Instance.GetObject(ObjectType.InventorySlot).GetComponent<InventorySlot>();
                slot.transform.SetParent(m_slot_parent);
                slot.AddItem(selected_item);
                m_slots.Add(slot);
            }

            yield return new WaitForSeconds(0.2f);
        }
        m_exit_button.enabled = true;
    }
}
