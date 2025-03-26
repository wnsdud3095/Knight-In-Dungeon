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

    private bool m_equipment = false;

    private void Awake()
    {
        m_rect_transform = m_canvas.GetComponent<RectTransform>();
    }

    public void OpenUI(Item item, bool equipment = true)
    {
        m_slot.AddItem(item);

        m_name_label.text = ItemDataManager.Instance.GetName(item.ID);
        m_description_label.text = ItemDataManager.Instance.GetDescription(item.ID);

        m_equipment = equipment;
        m_button_label.text = m_equipment ? "장착" : "해제";

        m_tooltip_object.SetBool("Open", true);
    }

    public void Button_CloseUI()
    {        
        m_tooltip_object.SetBool("Open", false);
    }

    public void Button_Equipment()
    {
        if(m_equipment)
        {
            // 장착
        }
        else
        {
            // 해제
        }
    }
}
