using UnityEngine;

public abstract class PlayerSkillBase //인터페이스 말고 추상 클래스로 구현
{
    public int Level { get; protected set; } = 1;

    public void LevelUP()
    {
        Level++;
        ApplyLevelUpEffect(Level);
    }

    public abstract void UseSKill();
    protected abstract void ApplyLevelUpEffect(int level);
}
