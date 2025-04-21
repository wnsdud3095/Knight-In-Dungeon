using UnityEngine;

public class ESkill5_MagicMissile : Skill5_MagicMissile
{
    private float m_e_skill5_cool_time = 0.5f;
    private float m_damage_e_level_ratio = 2f; // 스킬 만랩의 레벨별 공격력 배수
    protected float m_e_throw_speed = 10f;

    new void Awake()
    {
        m_cool_time = m_e_skill5_cool_time;
        m_bullet = SkillBullet.IceBolt;
        m_damage_level_ratio = m_damage_e_level_ratio;
        m_throw_speed = m_e_throw_speed;
    }
}
