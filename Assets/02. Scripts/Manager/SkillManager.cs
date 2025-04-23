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
        {4, typeof(Skill5_MagicMissile)},
        {5, typeof(Skill6_PiercingLight)},
        {10, typeof(DamageBuffSkill)},
        {11, typeof(HpBuffSkill)},
        {12, typeof(ScaleBuffSKill)},
        {13, typeof(MoveSpeedBuffSkill)},
        {14, typeof(ExpBuffSKill)},
        {15, typeof(CoolTimeBuffSKll)},
        {20, typeof(ESkill1_KunaiThorw)},
        {21, typeof(ESkill2_Severing)},
        {22, typeof(ESkill3_SpinningShuriken)},
        {23, typeof(ESkill4_CallThunder)},
        {24, typeof(ESkill5_MagicMissile)},
        {25, typeof(ESkill6_PiercingLight)},
        {100, typeof(Skill1_HolySword)},
        {101, typeof(Skill2_ThrowAxe)},
        {102, typeof(Skill3_HolyWand)},
        {103, typeof(Skill4_HolyBow)}
    };

    private int m_max_using_skill = 9;

    void Start()
    {
        GetWeaponSkill(GameManager.Instance.CalculatedStat.WeaponID);
        AddSkill(21);
    }

    public void GetWeaponSkill(int id)
    {
        int weapon = id / 10;
        int level = id % 10;
        int skill_code = 0;
        switch (weapon)
        {
            case 0:
                return;
            case 4:
                AddSkill(100);
                skill_code = 100;
                break;
            case 5:
                AddSkill(101);
                skill_code = 101;
                break;
            case 6:
                AddSkill(102);
                skill_code = 102;
                break;
            case 7:
                AddSkill(103);
                skill_code = 103;
                break;
        }
        for (int i = 0; i < level; i++)
        {
            GetSkillBase(skill_code).LevelUP();
        }
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

    public void RemoveSkill(int skill_id)
    {
        if (m_skill_map.TryGetValue(skill_id, out var type))
        {
            var old_skill = gameObject.GetComponent(type);
            Destroy(old_skill);
            UsingSKills.Remove(old_skill as PlayerSkillBase);
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

    public void SkillEvolve(int skill_id)
    {
        RemoveSkill(skill_id);
        AddSkill(skill_id + 20);
    }
}
