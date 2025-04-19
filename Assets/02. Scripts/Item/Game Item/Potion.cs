using Fusion;
using UnityEngine;

public class Potion : NetworkBehaviour, IItem
{
    public void Use(PlayerCtrl player_ctrl)
    {
        player_ctrl.UpdateHP(player_ctrl.OriginStat.HP);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            NetworkObject player_object = collision.GetComponent<NetworkObject>();
            if(GameManager.Instance.Player1.GetComponent<NetworkObject>().InputAuthority == player_object.InputAuthority)
            {
                Use(GameManager.Instance.Player1);
            }
            else
            {
                Use(GameManager.Instance.Player2);
            }

            Runner.Despawn(GetComponent<NetworkObject>());
        }
    }
}
