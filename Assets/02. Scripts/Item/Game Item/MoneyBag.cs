using UnityEngine;
using Fusion;

public class MoneyBag : NetworkBehaviour, IItem
{
    public void Use(PlayerCtrl player_ctrl)
    {
        GameObject.Find("Stage Manager").GetComponent<StageManager>().Kill += UnityEngine.Random.Range(100, 500);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Use(collision.GetComponent<PlayerCtrl>());

            Runner.Despawn(GetComponent<NetworkObject>());            
        }
    }
}