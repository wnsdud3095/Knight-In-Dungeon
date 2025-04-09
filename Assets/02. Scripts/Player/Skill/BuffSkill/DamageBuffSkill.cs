using UnityEngine;

public class DamageBuffSkill : PlayerSkillBase
{
    public float m_damage_buff = 1.2f;

    private float m_buff_increase = 0.2f;

    public override void UseSKill()
    {
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        GameManager.Instance.Player.Stat.AtkDamage = GameManager.Instance.Player.OriginStat.AtkDamage * m_damage_buff;
        m_damage_buff += m_buff_increase;
    }
}
