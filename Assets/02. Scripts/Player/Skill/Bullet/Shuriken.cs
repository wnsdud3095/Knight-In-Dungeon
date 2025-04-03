using UnityEngine;

public class Shuriken : Kunai
{

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
