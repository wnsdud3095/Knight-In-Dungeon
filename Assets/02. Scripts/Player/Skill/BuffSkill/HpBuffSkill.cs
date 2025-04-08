using UnityEngine;

public class HpBuffSkill : PlayerSkillBase
{
    public float HpBuff { get; private set; } = 50f;

    private float m_buff_increase = 30f;

    public override void UseSKill()
    {
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        HpBuff += m_buff_increase;
    }
}
