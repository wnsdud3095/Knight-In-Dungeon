using UnityEngine;

public class ESkill3_SpinningShuriken : Skill3_SpinningShuriken
{
    private int m_e_shuriken_count = 8;
    private float m_e_spinning_spped = 250f;
    private float m_e_spinning_radius = 2f;
    private float m_e_life_time = float.PositiveInfinity;
    private float m_e_damage = GameManager.Instance.Player.Stat.AtkDamage * 2.5f;

    private bool m_is_started = false;
    protected override void Start()
    {
        base.Start();
        m_shuriken_count = m_e_shuriken_count;
        m_spinning_spped = m_e_spinning_spped;
        m_spinning_radius = m_e_spinning_radius;
        m_life_time = m_e_life_time;
        m_damage = m_e_damage;
        m_is_started = true;
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
