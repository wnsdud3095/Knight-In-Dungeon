using UnityEngine;
using UnityEngine.UI;

public class Setter : MonoBehaviour
{
    [Header("배경음 토글")]
    [SerializeField] private Toggle m_bgm_toggle;

    [Header("효과음 토글")]
    [SerializeField] private Toggle m_sfx_toggle;

    [Header("진동 토글")]
    [SerializeField] private Toggle m_vibe_toggle;

    [Header("조이스틱 토글")]
    [SerializeField] private Toggle m_joystick_toggle;

    [Header("데미지 토글")]
    [SerializeField] private Toggle m_damage_toggle;

    private void Awake()
    {
        m_bgm_toggle.isOn = SettingManager.Instance.Data.BGM;
        m_sfx_toggle.isOn = SettingManager.Instance.Data.SFX;
        m_vibe_toggle.isOn = SettingManager.Instance.Data.Vibration;

        if(m_joystick_toggle is not null)
        {
            m_joystick_toggle.isOn = SettingManager.Instance.Data.JoyStick;
        }

        if(m_damage_toggle is not null)
        {
            m_damage_toggle.isOn = SettingManager.Instance.Data.Damage;   
        }
    }

    public void Toggle_BGM()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        SettingManager.Instance.Data.BGM = m_bgm_toggle.isOn;

        if(!SettingManager.Instance.Data.BGM)
        {
            string clip_name = "";
            switch(LoadingManager.Instance.Current)
            {
                case "Title":
                    clip_name = "Title Background";
                    break;
                
                case "Jongmin":
                    clip_name = "Game Background";
                    break;
            }

            if(SoundManager.Instance.BGM.clip is null)
            {
                SoundManager.Instance.PlayBGM(clip_name);
            }
            else
            {
                if(clip_name == SoundManager.Instance.BGM.clip.name)
                {
                    SoundManager.Instance.BGM.UnPause();
                }
                else
                {
                    SoundManager.Instance.BGM.clip = null;
                    SoundManager.Instance.PlayBGM(clip_name);
                }
            }
        }
        else
        {
            SoundManager.Instance.BGM.Pause();
        }
    }

    public void Toggle_SFX()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        SettingManager.Instance.Data.SFX = m_sfx_toggle.isOn;
    }

    public void Toggle_VIBE()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        SettingManager.Instance.Data.Vibration = m_vibe_toggle.isOn;

        // TODO: 진동 출력
    }

    public void Toggle_Joystick()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        SettingManager.Instance.Data.JoyStick = m_joystick_toggle.isOn;
    }

    public void Toggle_Damage()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        SettingManager.Instance.Data.Damage = m_damage_toggle.isOn;
    }

    public void Button_Title()
    {
        LoadingManager.Instance.LoadScene("Title");
    }

    public void Button_Exit()
    {
        GameManager.Instance.Save();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
