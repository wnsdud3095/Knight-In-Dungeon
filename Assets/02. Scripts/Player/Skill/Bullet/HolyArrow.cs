using UnityEngine;

public class HolyArrow : MagicMissile
{
    private float m_throw_speed = 5.5f;
    protected override void Awake()
    {
        base.Awake();
        Speed = m_throw_speed;
    }
}
