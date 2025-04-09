using UnityEngine;

public class MoveSpeedBuffSkill : PlayerSkillBase
{
    public float m_move_speed_buff = 1.2f;

    private float m_buff_increase = 0.2f;
    public override void UseSKill()
    {
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        GameManager.Instance.Player.Stat.MoveSpeed = GameManager.Instance.Player.OriginStat.MoveSpeed * m_move_speed_buff;
        m_move_speed_buff += m_buff_increase;
    }
}
