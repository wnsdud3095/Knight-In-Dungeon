using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public List<PlayerSkillBase> UsingSKills { get; set; } = new List<PlayerSkillBase>();

    private int m_max_using_skill = 4;

    void Start()
    {
        //AddSkill<ESkill1_KunaiThorw>();
        //AddSkill<ESkill2_Severing>();
        AddSkill<Skill3_SpinningShuriken>();
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

    public void UseSkills()
    {
        if (UsingSKills == null) return;

        foreach(var skill in UsingSKills)
        {
            skill.UseSKill();
        }
    }

}
