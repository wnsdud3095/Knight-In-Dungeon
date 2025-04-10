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

    [Space(30)][Header("체력 UI")]
    [Header("체력 UI 캔버스 그룹")]
    [SerializeField] private CanvasGroup m_hp_group;
    
    [Header("체력 UI 슬라이더")]
    [SerializeField] private Slider m_hp_slider;

    public void Update()
    {
        m_exp_slider.value = Mathf.Clamp(GameManager.Instance.StageManager.CurrentExp / GameManager.Instance.StageManager.MaxExp, 0f, 1f);
        m_hp_slider.value = GameManager.Instance.Player.Stat.HP / GameManager.Instance.Player.OriginStat.HP;

        if(GameManager.Instance.GameState is not GameEventType.Playing)
        {
            return;
        }

        m_play_time_label.text = (GameManager.Instance.StageManager.GameTimer / 60).ToString("00") + ":" + (GameManager.Instance.StageManager.GameTimer % 60).ToString("00");

        m_level_label.text = $"LV.{GameManager.Instance.StageManager.Level}";
        
        m_money_label.text = GameManager.Instance.StageManager.Kill.ToString("00000");

        if(m_hp_slider.value == 1f)
        {
            m_hp_group.alpha = 0f;
        }
        else
        {
            m_hp_group.alpha = 1f;
        }
    }

    public void Button_SettingEnter()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        GameEventBus.Publish(GameEventType.Setting);

        m_setting_ui_obect.SetBool("Open", true);
    }

    public void Button_SettingExit()
    {
        SoundManager.Instance.PlayEffect("Button Click");
        
        SettingManager.Instance.SaveSettingData();

        GameEventBus.Publish(GameEventType.Playing);

        m_setting_ui_obect.SetBool("Open", false);
    }
}
