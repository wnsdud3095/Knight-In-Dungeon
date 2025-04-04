using UnityEngine;

public class Shuriken : Kunai
{
    public float LifeTime { get {return m_life_time; }  set { m_life_time =value; } }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameEventType.Playing) return;
        LifeTimeCheck();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //데미지
            //넉백
        }
    }

}
