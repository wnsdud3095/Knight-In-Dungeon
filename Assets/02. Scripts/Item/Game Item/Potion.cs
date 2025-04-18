using Fusion;
using UnityEngine;

public class Potion : NetworkBehaviour, IItem
{
    public void Use(NetworkObject player_object)
    {
        var player = player_object.GetComponent<PlayerCtrl>();

        if(player)
        {
            player.UpdateHP(player.OriginStat.HP);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!HasStateAuthority)
        {
            return;
        }

        if(collision.CompareTag("Player"))
        {
            NetworkObject player_object = collision.GetComponent<NetworkObject>();

            if(player_object)
            {
                Use(player_object);
                GameManager.Instance.NowRunner.Despawn(GetComponent<NetworkObject>());
            }
        }
    }
}
