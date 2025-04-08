using UnityEngine;

public class MoveSpeedBuffSkill : PlayerSkillBase
{
    public float MoveSpeedBuff { get; private set; } = 1.2f;

    private float m_buff_increase = 0.2f;
    public override void UseSKill()
    {
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        MoveSpeedBuff += m_buff_increase;
    }
}
