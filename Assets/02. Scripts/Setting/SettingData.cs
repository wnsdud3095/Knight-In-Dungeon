using UnityEngine;

[System.Serializable]
public class SettingData
{
    [SerializeField] private bool m_bgm_print;
    public bool BGM
    {
        get { return m_bgm_print; }
        set { m_bgm_print = value; }
    }

    [SerializeField] private bool m_sfx_print;
    public bool SFX
    {
        get { return m_sfx_print; }
        set { m_sfx_print = value; }
    }

    [SerializeField] private bool m_vibration_print;
    public bool Vibration
    {
        get { return m_vibration_print; }
        set { m_vibration_print = value; }
    }

    [SerializeField] private bool m_joystick_print;
    public bool JoyStick
    {
        get { return m_joystick_print; }
        set { m_joystick_print = value; }
    }

    [SerializeField] private bool m_damage_print;
    public bool Damage
    {
        get { return m_damage_print; }
        set { m_damage_print = value; }
    }

    public SettingData()
    {
        BGM = true;
        SFX = true;
        Vibration = true;
        JoyStick = true;
        Damage = true;
    }
}