using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    [Header("스킬 선택 UI의 애니메이터")]
    [SerializeField] private Animator m_selector_ui_animator;

    [Header("스킬 선택 슬롯 UI의 부모 트랜스폼")]
    [SerializeField] private Transform m_slot_parent;

    [Header("공격 스킬 슬롯 UI의 부모 트랜스폼")]
    [SerializeField] private Transform m_active_skill_slot_parent;
    private SkillSlot[] m_active_skill_slots;
    public SkillSlot[] ActiveSkillSlots
    {
        get { return m_active_skill_slots; }
    }

    [Header("버프 스킬 슬롯 UI의 부모 트랜스폼")]
    [SerializeField] private Transform m_passive_skill_slot_parent;
    private SkillSlot[] m_passive_skill_slots;
    public SkillSlot[] PassiveSkillSlots
    {
        get { return m_passive_skill_slots; }
    }

    private SkillSelectSlot[] m_skill_select_slots;

    private List<int> m_selected_slots;

    private void Awake()
    {
        m_skill_select_slots = m_slot_parent.GetComponentsInChildren<SkillSelectSlot>();
        m_active_skill_slots = m_active_skill_slot_parent.GetComponentsInChildren<SkillSlot>();
        m_passive_skill_slots = m_passive_skill_slot_parent.GetComponentsInChildren<SkillSlot>();
        
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

        if(CheckCanSelect())
        {
            SetRandomSkills();
        }
        else
        {
            if(CheckAllMaxEvolution() is false)
            {
                SetSelectedSkills();
            }
            else
            {
                // TODO: 체력 회복.
                CloseUI();
            }
        }
    }

    private bool CheckCanSelect()
    {
        foreach(SkillSlot slot in ActiveSkillSlots)
        {
            if(slot.Skill is null)
            {
                return true;
            }
        }

        foreach(SkillSlot slot in PassiveSkillSlots)
        {
            if(slot.Skill is null)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckAllMaxEvolution()
    {
        foreach(SkillSlot slot in ActiveSkillSlots)
        {
            foreach(SkillSelectSlot select_slot in m_skill_select_slots)
            {
                if(slot.Skill.ID == select_slot.Skill.ID)
                {
                    if(select_slot.Base.Level < 6)
                    {
                        return false;
                    }
                }
            }
        }

        foreach(SkillSlot slot in PassiveSkillSlots)
        {
            foreach(SkillSelectSlot select_slot in m_skill_select_slots)
            {
                if(slot.Skill.ID == select_slot.Skill.ID)
                {
                    if(select_slot.Base.Level < 5)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private void SetRandomSkills()
    {
        while(m_selected_slots.Count < 3)
        {
            int random_idx = UnityEngine.Random.Range(0, m_skill_select_slots.Length);

            if(m_selected_slots.Contains(random_idx))
            {
                continue;
            }

            if(m_skill_select_slots[random_idx].Base is not null)
            {
                if(m_skill_select_slots[random_idx].Base.Level >= 6)
                {
                    continue;
                }
            }

            m_selected_slots.Add(random_idx);
        }

        foreach(int index in m_selected_slots)
        {
            m_skill_select_slots[index].Initialize();
            
            if(m_skill_select_slots[index].Base is not null)
            {
                if(m_skill_select_slots[index].Base.Level == 5)
                {
                    m_skill_select_slots[index].ChangeToEvolution();
                }
            }

            m_skill_select_slots[index].gameObject.SetActive(true);                
        }
    }

    private void SetSelectedSkills()
    {
        List<int> evolvable_slots = new List<int>();

        for (int i = 0; i < m_skill_select_slots.Length; i++)
        {
            var select_slot = m_skill_select_slots[i];
            var skill = select_slot.Skill;

            if (skill == null || select_slot.Base == null)
            {
                continue;
            }

            bool has_skill = false;
            bool can_level_up = false;

            if (skill.Type == SkillType.Active)
            {
                foreach (SkillSlot slot in ActiveSkillSlots)
                {
                    if (slot.Skill != null && slot.Skill.ID == skill.ID)
                    {
                        has_skill = true;
                        can_level_up = select_slot.Base.Level < 6;
                        break;
                    }
                }
            }
            else
            {
                foreach (SkillSlot slot in PassiveSkillSlots)
                {
                    if (slot.Skill != null && slot.Skill.ID == skill.ID)
                    {
                        has_skill = true;
                        can_level_up = select_slot.Base.Level < 5;
                        break;
                    }
                }
            }

            if (has_skill && can_level_up)
            {
                evolvable_slots.Add(i);
            }
        }

        foreach (int index in evolvable_slots)
        {
            var slot = m_skill_select_slots[index];
            m_selected_slots.Add(index);

            slot.Initialize();

            if (slot.Base.Level == 5)
            {
                slot.ChangeToEvolution();
            }

            slot.gameObject.SetActive(true);
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