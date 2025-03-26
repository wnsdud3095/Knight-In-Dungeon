using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryTooltip : MonoBehaviour
{
    [Header("툴팁 UI 오브젝트")]
    [SerializeField] private GameObject m_tooltip_object;

    [Header("툴팁 UI 오브젝트가 위치할 캔버스")]
    [SerializeField] private Canvas m_canvas;

    [SerializeField] private TMP_Text m_name_label;
    [SerializeField] private TMP_Text m_description_label;
    [SerializeField] private TMP_Text m_button_label;

    private void Update()
    {
        if(m_tooltip_object.activeInHierarchy)
        {
            CalculateTouchPosition();
        }
    }

    public void OpenUI(int item_id)
    {
        // 툴팁 데이터 설정

        m_tooltip_object.SetActive(true);
    }

    public void CloseUI()
    {
        m_tooltip_object.SetActive(false);
    }
    
    private void CalculateTouchPosition()
    {
        // 마우스 위치 계산
    }
}
