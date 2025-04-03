using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUICtrl : MonoBehaviour
{
    [Header("게임 UI")]
    [Header("플레이 타임 라벨")]
    [SerializeField] private TMP_Text m_play_time_label;

    [Header("돈(몬스터) 라벨")]
    [SerializeField] private TMP_Text m_money_label;

    [Header("현재 레벨 라벨")]
    [SerializeField] private TMP_Text m_level_label;

    [Header("현재 경험치 슬라이더")]
    [SerializeField] private Slider m_exp_slider;

    [Space(30)][Header("설정 UI")]
    [Header("설정 UI 애니메이터")]
    [SerializeField] private Animator m_setting_ui_obect;

    [SerializeField] private StageManager m_stage_manager;

    public void Update()
    {
        if(GameManager.Instance.GameState is not GameEventType.Playing)
        {
            return;
        }

        m_play_time_label.text = (m_stage_manager.GameTimer / 60).ToString("00") + ":" + (m_stage_manager.GameTimer % 60).ToString("00");

        m_level_label.text = $"LV.{m_stage_manager.Level}";
        m_exp_slider.value = Mathf.Clamp(m_stage_manager.CurrentExp / m_stage_manager.MaxExp, 0f, 1f);
        m_money_label.text = m_stage_manager.Kill.ToString("00000");
    }

    public void Button_SettingEnter()
    {
        GameEventBus.Publish(GameEventType.Setting);

        m_setting_ui_obect.SetBool("Open", true);
    }

    public void Button_SettingExit()
    {
        SettingManager.Instance.SaveSettingData();

        GameEventBus.Publish(GameEventType.Playing);

        m_setting_ui_obect.SetBool("Open", false);
    }
}
