using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ItemDataManager : Singleton<ItemDataManager>
{
    private string m_item_data_path;

    private Dictionary<int, string> m_item_name_dictionary;
    private Dictionary<int, string> m_item_description_dictionary;

    [Header("아이템 스크립터블 오브젝트 목록")]
    [SerializeField] private Item[] m_item_data_list;
    public Item[] ItemDataList
    {
        get { return m_item_data_list; }
    }

    private new void Awake()
    {
        base.Awake();

        m_item_data_path = Path.Combine(Application.streamingAssetsPath, "ItemData.json");

        m_item_name_dictionary = new Dictionary<int, string>();
        m_item_description_dictionary = new Dictionary<int, string>();

        Initialize();
    }

    private void Initialize()
    {
        if(File.Exists(m_item_data_path))
        {
            var json_data = File.ReadAllText(m_item_data_path);
            var item_list = JsonUtility.FromJson<ItemInfos>(json_data);

            if(item_list is not null && item_list.m_item_infos is not null)
            {
                foreach(var item in item_list.m_item_infos)
                {
                    m_item_name_dictionary.Add(item.m_item_id, item.m_item_name);
                    m_item_description_dictionary.Add(item.m_item_id, item.m_item_description);
                }
            }
        }
        else
        {
            Debug.Log($"{m_item_data_path}가 존재하지 않습니다.");
        }
    }

    public string GetName(int item_id)
    {
        return m_item_name_dictionary.ContainsKey(item_id) ? m_item_name_dictionary[item_id] : null;
    }

    public string GetDescription(int item_id)
    {
        return m_item_description_dictionary.ContainsKey(item_id) ? m_item_description_dictionary[item_id] : null;
    }

    public Item GetItem(int item_id)
    {
        foreach(var item in m_item_data_list)
        {
            if(item.ID == item_id)
            {
                return item;
            }
        }

        return null;
    }
}

[System.Serializable]
public class ItemInfo
{
    public int m_item_id;
    public string m_item_name;
    public string m_item_description;
}

[System.Serializable]
public class ItemInfos
{
    public ItemInfo[] m_item_infos;
}
