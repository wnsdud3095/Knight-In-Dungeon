using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserStatusCtrl : MonoBehaviour
{
    [SerializeField] private TMP_Text m_money_label;

    [SerializeField] private TMP_Text m_level_label;
    [SerializeField] private Slider m_exp_slider;

    private void Update()
    {
        m_money_label.text = DataManager.Instance.Data.m_user_money.ToString();
        m_level_label.text = DataManager.Instance.Data.m_user_level.ToString();
        m_exp_slider.value = 0.3f;   
    }
}
