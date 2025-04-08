using UnityEngine;

public class HpBuffSkill : PlayerSkillBase
{
    public float m_hp_buff= 50f;

    private float m_buff_increase = 30f;

    public override void UseSKill()
    {
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        GameManager.Instance.Player.OriginStat.HP += m_hp_buff;
        GameManager.Instance.Player.Stat.HP += m_hp_buff;
        m_hp_buff += m_buff_increase;
    }
}
