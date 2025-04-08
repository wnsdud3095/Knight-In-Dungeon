using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public List<PlayerSkillBase> UsingSKills { get; set; } = new List<PlayerSkillBase>();
    private Dictionary<int, System.Type> m_skill_map = new Dictionary<int, System.Type>()
    {
        {0, typeof(Skill1_KunaiThorw)},
        {1, typeof(Skill2_Severing)},
        {2, typeof(Skill3_SpinningShuriken)},
        {3, typeof(Skill4_CallThunder)},
        {4, typeof(Skill1_KunaiThorw)},
        {5, typeof(Skill1_KunaiThorw)},
        {10, typeof(Skill1_KunaiThorw)},
        {11, typeof(Skill1_KunaiThorw)},
        {12, typeof(Skill1_KunaiThorw)},
        {13, typeof(Skill1_KunaiThorw)},
        {14, typeof(Skill1_KunaiThorw)},
        {15, typeof(Skill1_KunaiThorw)}
    };

    private int m_max_using_skill = 4;

    void Start()
    {
        //AddSkill<Skill1_KunaiThorw>();
        //AddSkill<DamageBuffSkill>();
        AddSkill<Skill2_Severing>();
        //AddSkill<ESkill3_SpinningShuriken>();
        //AddSkill<ESkill4_CallThunder>();
        //AddSkill<ESkill5_MagicMissile>();
        //AddSkill<ESkill6_PiercingLight>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSkill<T>() where T : PlayerSkillBase // 동적으로 스킬 추가
    {
        if (UsingSKills.Count < m_max_using_skill)
        {
            PlayerSkillBase new_skill = gameObject.AddComponent<T>(); // 반드시 게임 오브젝트에 컴포넌트 추가 해야함 오브젝트가 없으면 모노비헤이비어 기반 함수를 사용 불가
            UsingSKills.Add(new_skill);
            Debug.Log($"{typeof(T).Name} 스킬 추가");
        }
    }

    public void AddSkill(int skill_id)
    {
        if(UsingSKills.Count >= m_max_using_skill)
        {
            return;
        }

        if(m_skill_map.TryGetValue(skill_id, out var type))
        {
            var new_skill = (PlayerSkillBase)gameObject.AddComponent(type);
            UsingSKills.Add(new_skill);
            Debug.Log($"{type.Name} 스킬 추가");
        }
    } 

    public void UseSkills()
    {
        if (UsingSKills == null) return;

        foreach(var skill in UsingSKills)
        {
            skill.UseSKill();
        }
    }

    public PlayerSkillBase GetSkillBase(int skill_id)
    {
        if(m_skill_map.TryGetValue(skill_id, out var type))
        {
            return (PlayerSkillBase)gameObject.GetComponent(type);
        }

        return null;
    }
}
