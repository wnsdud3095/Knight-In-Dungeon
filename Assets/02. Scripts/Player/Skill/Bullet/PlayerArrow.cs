using UnityEngine;

public class PlayerArrow : MagicMissile
{
    private float m_throw_speed = 5.5f;
    protected override void Awake()
    {
        Speed = m_throw_speed;
    }

    protected new void Update()
    {
        if (GameManager.Instance.GameState != GameEventType.Playing) return;

        transform.Translate(Vector3.up * Speed * Time.deltaTime);

        PerCheck();
        LifeTimeCheck();
    }
}
