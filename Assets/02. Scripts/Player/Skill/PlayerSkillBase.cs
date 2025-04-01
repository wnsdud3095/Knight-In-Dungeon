using UnityEngine;
using System.Collections;

public abstract class PlayerSkillBase  : MonoBehaviour//인터페이스 말고 추상 클래스로 구현
{
    public int Level { get; protected set; } = 1;

    protected bool m_can_use = true;

    protected float m_cool_time = 2f;
    protected float m_cool_down_time = 0;

    public void LevelUP()
    {
        Level++;
        ApplyLevelUpEffect(Level);
    }

    protected void CoolTime(float cool_time)
    {
        if (m_cool_down_time < cool_time)
        {
            m_cool_down_time += Time.deltaTime;
            if (m_can_use) m_can_use = false;
        }
        else
        {
            m_can_use = true;
            m_cool_down_time = 0;
        }
    }
    public abstract void UseSKill();
    protected abstract void ApplyLevelUpEffect(int level);
}
