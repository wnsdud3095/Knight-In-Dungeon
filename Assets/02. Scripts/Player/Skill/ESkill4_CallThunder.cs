using UnityEngine;

public class ESkill4_CallThunder : Skill4_CallThunder
{
    private float m_e_skill4_cool_time = 2f;
    private int m_e_thunder_count = 4;
    private float m_damage_e_level_ratio = 2f; // 스킬 만랩의 레벨별 공격력 배수

    protected override void Start()
    {
        base.Start();
        m_cool_time = m_e_skill4_cool_time;
        m_thunder_count = m_e_thunder_count;
        m_bullet = SkillBullet.ThunderBolt;
        m_damage_level_ratio = m_damage_e_level_ratio;
    }

}
