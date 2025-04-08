using UnityEngine;

public class ExpBuffSKill : PlayerSkillBase
{
    public float ExpBuff { get; private set; } = 1.2f;

    private float m_buff_increase = 0.2f;
    public override void UseSKill()
    {
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        ExpBuff += 0.2f;
    }
}
