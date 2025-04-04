using UnityEngine;

public class ESkill4_CallThunder : Skill4_CallThunder
{
    private float m_e_skill4_cool_time = 2f;
    private int m_e_thunder_count = 4;

    private float m_e_damage = GameManager.Instance.Player.Stat.AtkDamage * 5;
    

    protected override void Start()
    {
        base.Start();
        m_cool_time = m_e_skill4_cool_time;
        m_thunder_count = m_e_thunder_count;
        m_damage = m_e_damage;
        m_bullet = SkillBullet.ThunderBolt;
    }

}
