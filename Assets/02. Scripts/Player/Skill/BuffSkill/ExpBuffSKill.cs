using UnityEngine;

public class ExpBuffSKill : PlayerSkillBase
{
    public float m_exp_buff = 1.2f;

    private float m_buff_increase = 0.2f;
    public override void UseSKill()
    {
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        GameManager.Instance.Player.Stat.ExpBonusRatio = m_exp_buff;
        m_exp_buff += m_buff_increase;
    }
}
