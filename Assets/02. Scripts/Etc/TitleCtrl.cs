using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleCtrl : MonoBehaviour
{
    [Header("장비 인벤토리")]
    [SerializeField] private EquipmentInventory m_equipment_inventory;

    [Header("능력치 강화")]
    [SerializeField] private Evolution m_evolution;

    [Space(50)] [Header("UI 관련 컴포넌트")]
    [Header("상점 토글")]
    [SerializeField] private Toggle m_shop_toggle;

    [Header("인벤토리 토글")]
    [SerializeField] private Toggle m_inventory_toggle;

    [Header("게임 토글")]
    [SerializeField] private Toggle m_game_toggle;

    [Header("업그레이드 토글")]
    [SerializeField] private Toggle m_upgrade_toggle;

    [Space(30)]
    [Header("상점 패널")]
    [SerializeField] private Animator m_shop_panel;

    [Header("인벤토리 패널")]
    [SerializeField] private Animator m_inventory_panel;

    [Header("업그레이드 패널")]
    [SerializeField] private Animator m_upgrade_panel;

    [Header("설정 패널")]
    [SerializeField] private Animator m_setting_panel;

    [Space(30)][Header("메뉴 UI 컴포넌트")]
    [SerializeField] private TMP_Text m_money_label;

    [SerializeField] private TMP_Text m_level_label;
    [SerializeField] private Slider m_exp_slider;


    [Header("스테이지 라벨")]
    [SerializeField] private TMP_Text m_stage_label;

    private void Awake()
    {
        GameEventBus.Publish(GameEventType.Waiting);

        m_stage_label.text = $"스테이지 {DataManager.Instance.Data.m_current_stage}";
    
        m_level_label.text = $"LV.{DataManager.Instance.Data.m_user_level}";
    }

    private void Update()
    {
        m_money_label.text = NumberFormatter.FormatNumber(DataManager.Instance.Data.m_user_money);
    }

    public void SetCalculatedStat()
    {
        GameManager.Instance.CalculatedStat = new CalculatedStat(0f, 0f, 0f);

        GameManager.Instance.CalculatedStat.HP += m_equipment_inventory.EquipmentEffect.HP;
        GameManager.Instance.CalculatedStat.ATK += m_equipment_inventory.EquipmentEffect.ATK;

        foreach(EvolutionSlot slot in m_evolution.Slots)
        {
            if(slot.Level > DataManager.Instance.Data.m_evolution_level)
            {
                break;
            }

            switch(slot.Type)
            {
                case EvolutionType.HP:
                    GameManager.Instance.CalculatedStat.HP += slot.Rate;
                    break;
                
                case EvolutionType.ATK:
                    GameManager.Instance.CalculatedStat.ATK += slot.Rate;
                    break;
                
                case EvolutionType.HP_REGEN:
                    GameManager.Instance.CalculatedStat.HP_REGEN += slot.Rate;
                    break;
            }
        }

        Debug.Log($"HP:{GameManager.Instance.CalculatedStat.HP}\nATK:{GameManager.Instance.CalculatedStat.ATK}\nHP_REGEN:{GameManager.Instance.CalculatedStat.HP_REGEN}");
    }

    public void Toggle_Shop()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        m_shop_panel.SetBool("Open", true);
        m_inventory_panel.SetBool("Open", false);
        m_upgrade_panel.SetBool("Open", false);
    }

    public void Toggle_Inventory()
    {
        SoundManager.Instance.PlayEffect("Button Click");
        
        m_shop_panel.SetBool("Open", false);
        m_inventory_panel.SetBool("Open", true);
        m_upgrade_panel.SetBool("Open", false);
    }

    public void Toggle_Game()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        m_shop_panel.SetBool("Open", false);
        m_inventory_panel.SetBool("Open", false);
        m_upgrade_panel.SetBool("Open", false);
    }

    public void Toggle_Upgrade()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        m_shop_panel.SetBool("Open", false);
        m_inventory_panel.SetBool("Open", false);
        m_upgrade_panel.SetBool("Open", true);
    }

    public void Button_SettingEnter()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        m_setting_panel.SetBool("Open", true);
    }

    public void Button_SettingExit()
    {
        SoundManager.Instance.PlayEffect("Button Click");
        
        m_setting_panel.SetBool("Open", false);
        SettingManager.Instance.SaveSettingData();
    }

    public void Button_SinglePlay()
    {
        SoundManager.Instance.PlayEffect("Button Click");
        GameManager.Instance.Save();
        LoadingManager.Instance.LoadScene("Jongmin");
    }

    public void Button_MultiPlay()
    {
    
    }
}
