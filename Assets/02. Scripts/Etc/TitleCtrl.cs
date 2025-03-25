using UnityEngine;
using UnityEngine.UI;

public class TitleCtrl : MonoBehaviour
{
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

    public void Toggle_Shop()
    {
        m_shop_panel.SetBool("Open", true);
        m_inventory_panel.SetBool("Open", false);
        m_upgrade_panel.SetBool("Open", false);
    }

    public void Toggle_Inventory()
    {
        m_shop_panel.SetBool("Open", false);
        m_inventory_panel.SetBool("Open", true);
        m_upgrade_panel.SetBool("Open", false);
    }

    public void Toggle_Game()
    {
        m_shop_panel.SetBool("Open", false);
        m_inventory_panel.SetBool("Open", false);
        m_upgrade_panel.SetBool("Open", false);
    }

    public void Toggle_Upgrade()
    {
        m_shop_panel.SetBool("Open", false);
        m_inventory_panel.SetBool("Open", false);
        m_upgrade_panel.SetBool("Open", true);
    }

    public void Button_SettingEnter()
    {
        m_setting_panel.SetBool("Open", true);
    }

    public void Button_SettingExit()
    {
        m_setting_panel.SetBool("Open", false);
    }
}
