using UnityEngine;

public class ESkill3_SpinningShuriken : Skill3_SpinningShuriken
{
    private int m_e_shuriken_count = 8;
    private float m_e_spinning_spped = 250f;
    private float m_e_spinning_radius = 2f;
    private float m_e_life_time = float.PositiveInfinity;
    private float m_damage_e_level_ratio = 2f; // 스킬 만랩의 레벨별 공격력 배수

    private bool m_is_started = false;
    protected override void Start()
    {
        base.Start();
        m_shuriken_count = m_e_shuriken_count;
        m_spinning_spped = m_e_spinning_spped;
        m_spinning_radius = m_e_spinning_radius;
        m_life_time = m_e_life_time;
        m_is_started = true;
        m_damage_level_ratio = m_damage_e_level_ratio;
    }

    public override void UseSKill()
    {
        if (!m_is_started) return;
        if (m_can_use)
        {
            SpawnShuriken();
        }
        CoolTime(float.PositiveInfinity);
    }

}
