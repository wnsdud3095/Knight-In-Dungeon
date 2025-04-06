using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public List<PlayerSkillBase> UsingSKills { get; set; } = new List<PlayerSkillBase>();

    private int m_max_using_skill = 4;

    void Start()
    {
        AddSkill<ESkill1_KunaiThorw>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSkill<T>() where T : PlayerSkillBase, new() // 동적으로 스킬 추가
    {
        if (UsingSKills.Count < m_max_using_skill)
        {
            PlayerSkillBase new_skill = gameObject.AddComponent<T>();
            UsingSKills.Add(new_skill);
            Debug.Log($"{typeof(T).Name} 스킬 추가");
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

}
