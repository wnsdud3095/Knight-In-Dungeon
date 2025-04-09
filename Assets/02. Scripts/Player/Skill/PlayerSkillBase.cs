using UnityEngine;
using System.Collections;

public abstract class PlayerSkillBase  : MonoBehaviour//인터페이스 말고 추상 클래스로 구현
{
    public int Level { get; protected set; } = 1;

    protected bool m_can_use = true;

    protected float m_cool_time = 2f;
    protected float m_cool_down_time = 0;

    private float m_max_level = 5;
    private SkillManager m_skill_manager;

    protected virtual void Awake()
    {
        m_skill_manager = GameObject.Find("Skill Manager").GetComponent<SkillManager>();
    }

    public void LevelUP()
    {
        Level++;
        ApplyLevelUpEffect(Level);
    }
    
    protected float GetFinallDamage(float m_skill_damage_ratio, float m_level_damage_ratio)
    {
        float finall_damage = GameManager.Instance.Player.Stat.AtkDamage * m_skill_damage_ratio * m_level_damage_ratio;
        return finall_damage;
    }

    protected void CoolTime(float cool_time)
    {
        cool_time *= GameManager.Instance.Player.Stat.CoolDownDecreaseRatio; // 쿨타임 감소 버프 값
        if (m_cool_down_time < cool_time)
        {
            m_cool_down_time += Time.deltaTime;
            if (m_can_use) m_can_use = false;
        }
        else
        {
            m_can_use = true;
            m_cool_down_time = 0;
        }
    }

    protected void CheckSkillEvolve(int skill_id)
    {
        if(Level > m_max_level)
        {
            m_skill_manager.SkillEvolve(skill_id);
        }
    }
    public abstract void UseSKill();
    protected abstract void ApplyLevelUpEffect(int level);
}
