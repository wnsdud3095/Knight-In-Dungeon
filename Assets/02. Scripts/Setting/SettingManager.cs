using System.IO;
using UnityEngine;

public class SettingManager : Singleton<SettingManager>
{
    private string m_setting_data_path;

    private SettingData m_setting_data;
    public SettingData Data
    {
        get { return m_setting_data; }
        set { m_setting_data = value; }
    }

    public new void Awake()
    {
        base.Awake();

        m_setting_data_path = Path.Combine(Application.persistentDataPath, "SettingData.json");

        if(File.Exists(m_setting_data_path))
        {
            LoadSettingData();
        }
        else
        {
            Data = new SettingData();
            SaveSettingData();
        }
    }

    public void LoadSettingData()
    {
        string json_data = File.ReadAllText(m_setting_data_path);

        if(json_data is not null)
        {
            Data = JsonUtility.FromJson<SettingData>(json_data);
        }
        else
        {
            Debug.LogError($"<color=blue>{m_setting_data_path}를 불러오는 데 실패했습니다.</color>");
        }
    }

    public void SaveSettingData()
    {
        string json_data = JsonUtility.ToJson(Data, true);

        File.WriteAllText(m_setting_data_path, json_data);

        Debug.Log($"<color=blue>{m_setting_data_path}를 세이브하는 데 성공했습니다.</color>");
    }
}
