using UnityEngine;

public class CoolTimeBuffSKll : PlayerSkillBase
{
    public float CoolTimeBuff { get; private set; } = 0.9f;

    private float m_buff_increase = 0.1f;
    public override void UseSKill()
    {
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        CoolTimeBuff -= m_buff_increase;
    }
}
