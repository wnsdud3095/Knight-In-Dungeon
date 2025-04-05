using System.Collections.Generic;
using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    [Header("스킬 선택 UI의 애니메이터")]
    [SerializeField] private Animator m_selector_ui_animator;

    [Header("스킬 선택 슬롯 UI의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_parent;

    private SkillSelectSlot[] m_skill_select_slots;

    private List<int> m_selected_slots;

    private void Awake()
    {
        m_skill_select_slots = m_slot_parent.GetComponentsInChildren<SkillSelectSlot>();
        m_selected_slots = new List<int>();
    }

    private void Start()
    {
        Initialize();
    }

    public void OpenUI()
    {
        GameEventBus.Publish(GameEventType.Selecting);

        m_selector_ui_animator.SetBool("Open", true);

        ActivateSlots();
    }

    public void CloseUI()
    {
        GameEventBus.Publish(GameEventType.Playing);
        
        m_selector_ui_animator.SetBool("Open", false);

        Initialize();
    }

    private void ActivateSlots()
    {
        Initialize();

        while(m_selected_slots.Count < 3)
        {
            // TOOD: 인벤에 있는 애들이 모두 만렙이라면 소비 아이템 바꿔치기.

            int random_idx = UnityEngine.Random.Range(0, m_skill_select_slots.Length);

            if(m_selected_slots.Contains(random_idx))
            {
                continue;
            }

            if(m_skill_select_slots[random_idx].Base?.Level > 6)
            {
                continue;
            }

            m_selected_slots.Add(random_idx);
        }

        foreach(int index in m_selected_slots)
        {
            m_skill_select_slots[index].Initialize();
            
            if(m_skill_select_slots[index].Base.Level == 5)
            {
                m_skill_select_slots[index].ChangeToEvolution();
            }

            m_skill_select_slots[index].gameObject.SetActive(true);
        }
    }

    private void DeactiveSlots()
    {
        foreach(SkillSelectSlot slot in m_skill_select_slots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    private void Initialize()
    {
        DeactiveSlots();
        m_selected_slots.Clear();
    }
}
