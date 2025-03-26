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

    public void OpenUI(Gacha gacha, int count)
    {
        m_prize_ui_object.SetBool("Open", true);
        List<Item> m_prize_item = new List<Item>();

        StartCoroutine(ShowCoroutine(gacha, count));
    }

    public void Button_CloseUI()
    {
        foreach(var slot in m_slots)
        {
            slot.transform.SetParent(GameObject.Find("Inventory Slot Container").transform);
            ObjectManager.Instance.ReturnObject(slot.gameObject, ObjectType.InventorySlot);
        }
        m_slots.Clear();
        
        m_prize_ui_object.SetBool("Open", false);
    }

    private IEnumerator ShowCoroutine(Gacha gacha, int count)
    {
        m_exit_button.enabled = false;

        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < count; i++)
        {
            int random = UnityEngine.Random.Range(0, gacha.Items.Length);
            Item item = gacha.Items[random];

            m_item_inventory.AcquireItem(item);

            InventorySlot slot = ObjectManager.Instance.GetObject(ObjectType.InventorySlot).GetComponent<InventorySlot>();
            slot.transform.SetParent(m_slot_parent);
            slot.AddItem(item);

            m_slots.Add(slot);

            yield return new WaitForSeconds(0.2f);
        }
        m_exit_button.enabled = true;
    }
}
