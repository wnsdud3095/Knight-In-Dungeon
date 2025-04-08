using UnityEngine;

public class CoolTimeBuffSKll : PlayerSkillBase
{
    public float m_cool_time_buff  = 0.9f;

    private float m_buff_increase = 0.1f;
    public override void UseSKill()
    {
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        GameManager.Instance.Player.Stat.CoolDownDecreaseRatio = m_cool_time_buff;
        m_cool_time_buff -= m_buff_increase;
    }
}
