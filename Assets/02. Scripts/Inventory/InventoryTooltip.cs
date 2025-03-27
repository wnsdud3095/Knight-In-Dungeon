using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryTooltip : MonoBehaviour
{
    [Header("툴팁 UI 오브젝트")]
    [SerializeField] private Animator m_tooltip_object;

    [Header("툴팁 UI 오브젝트가 위치할 캔버스")]
    [SerializeField] private Canvas m_canvas;
    private RectTransform m_rect_transform;

    [Space(30)]
    [Header("툴팁 UI 컴포넌트")]
    [SerializeField] private InventorySlot m_slot;
    [SerializeField] private TMP_Text m_name_label;
    [SerializeField] private int m_upgrade_count;
    [SerializeField] private TMP_Text m_description_label;
    [SerializeField] private TMP_Text m_button_label;

    [Space(30)]
    [Header("강화 UI 컴포넌트")]
    [SerializeField] private Reinforcer m_reinforcer;

    private InventorySlot m_current_slot;

    private void Awake()
    {
        m_rect_transform = m_canvas.GetComponent<RectTransform>();
    }

    public void OpenUI(Item item, InventorySlot current_slot, bool equipment = true)
    {
        m_slot.AddItem(item);

        m_name_label.text = $"<color=yellow>{ItemDataManager.Instance.GetName(item.ID)}</color>";
        m_description_label.text = ItemDataManager.Instance.GetDescription(item.ID);

        m_button_label.text = equipment ? "장착" : "해제";

        m_tooltip_object.SetBool("Open", true);

        m_current_slot = current_slot;
    }

    public void Button_CloseUI()
    {   
        m_current_slot = null;

        m_tooltip_object.SetBool("Open", false);
    }

    public void Button_Equipment()
    {
        m_current_slot.UseItem();
        Button_CloseUI();
    }

    public void Button_Reinforcement()
    {
        m_reinforcer.OpenUI(m_slot.Item);
    }
}
