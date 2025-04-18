using UnityEngine;
using Fusion;

public class MoneyBag : NetworkBehaviour, IItem
{
    public void Use(NetworkObject player_object)
    {
        GameObject.Find("Stage Manager").GetComponent<StageManager>().Kill += UnityEngine.Random.Range(100, 500);
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
                ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Item_MoneyBag);
            }
        }
    }
}