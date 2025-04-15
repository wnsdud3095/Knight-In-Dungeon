using TMPro;
using UnityEngine;

public class Finisher : MonoBehaviour
{
    [Header("게임 결과 UI 애니메이터")]
    [SerializeField] private Animator m_result_ui_animator;

    [Header("결과 라벨")]
    [SerializeField] private TMP_Text m_result_label;

    [Header("획득한 골드 라벨")]
    [SerializeField] private TMP_Text m_money_label;

    [Header("획득한 경험치 라벨")]
    [SerializeField] private TMP_Text m_exp_label;


    public void OpenUI(bool clear_flag)
    {
        m_result_ui_animator.SetBool("Open", true);

        m_result_label.text = "결과: ";
        if(clear_flag is true)
        {
            m_result_label.text += "<color=green>성공</color>";
        }
        else
        {
            m_result_label.text += "<color=red>실패</color>";
        }

        m_money_label.text = "획득한 골드: " + NumberFormatter.FormatNumber(GameManager.Instance.StageManager.Kill * DataManager.Instance.Data.m_current_stage);
        m_exp_label.text = "획득한 EXP: " + NumberFormatter.FormatNumber(Mathf.FloorToInt(GameManager.Instance.StageManager.OriginTimer - GameManager.Instance.StageManager.GameTimer));
    }

    public void Button_CloseUI()
    {
        m_result_ui_animator.SetBool("Open", false);

        LoadingManager.Instance.LoadScene("Title");
    }
}