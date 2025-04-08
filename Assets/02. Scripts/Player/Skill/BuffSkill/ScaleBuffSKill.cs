using UnityEngine;

public class ScaleBuffSKill : PlayerSkillBase
{
    public float ScaleBuff { get; private set; } = 1.2f;

    private float m_buff_increase = 0.2f;

    public override void UseSKill()
    {
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        ScaleBuff += m_buff_increase;
    }
}
