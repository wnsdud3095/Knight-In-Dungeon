using UnityEngine;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    [Header("가챠 슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_parent;

    [Header("가챠 슬롯의 프리펩")]
    [SerializeField] private GachaSlot m_slot_prefab; 

    [Header("상점에 등록된 가챠 목록")]
    [SerializeField] private Gacha[] m_gachas;

    private List<GachaSlot> m_gacha_list = new List<GachaSlot>();

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        RefreshSlots();
    }

    public void Initialize()
    {
        foreach(Gacha gacha in m_gachas)
        {
            GachaSlot slot = Instantiate(m_slot_prefab, m_slot_parent);
            slot.AddGacha(gacha);

            m_gacha_list.Add(slot);
        }

        RefreshSlots();
    }

    public void DestroySlot(Gacha gacha)
    {
        for(int i = 0; i < m_gacha_list.Count; i++)
        {
            if(m_gacha_list[i].Gacha.ID == gacha.ID)
            {
                m_gacha_list.RemoveAt(i);
                break;
            }
        }
    }

    public void RefreshSlots()
    {
        foreach(GachaSlot slot in m_gacha_list)
        {
            slot.Refresh();
        }
    }
}