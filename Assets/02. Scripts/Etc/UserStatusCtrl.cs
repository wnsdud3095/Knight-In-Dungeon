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
        m_exp_slider.value = DataManager.Instance.Data.m_user_exp / ExpData.m_exp_list[DataManager.Instance.Data.m_user_level % 10];

        if(m_exp_slider.value >= 1f)
        {
            DataManager.Instance.Data.m_user_exp = DataManager.Instance.Data.m_user_exp - ExpData.m_exp_list[DataManager.Instance.Data.m_user_level % 10];
            DataManager.Instance.Data.m_user_level++;
        }
    }
}
