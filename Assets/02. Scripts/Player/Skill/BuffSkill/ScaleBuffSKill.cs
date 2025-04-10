using UnityEngine;

public class ScaleBuffSKill : PlayerSkillBase
{
    public float m_scale_buff = 1.2f;

    private float m_buff_increase = 0.2f;

    public override void UseSKill()
    {
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        GameManager.Instance.Player.Stat.BulletSize = m_scale_buff;
        m_scale_buff += m_buff_increase;
    }
}
