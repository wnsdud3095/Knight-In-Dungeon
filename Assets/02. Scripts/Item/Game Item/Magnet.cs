using Fusion;
using UnityEngine;

public class Magnet : NetworkBehaviour, IItem
{
    public void Use(NetworkObject player_object)
    {
        GameObject[] m_exp_orbs = ObjectManager.Instance.GetActiveObjects(ObjectType.Exp);
        
        foreach(GameObject exp in m_exp_orbs)
        {
            exp.GetComponent<Exp>().Magneted = true;
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
            //Use();
            ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Item_Magnet);
        }
    }
}
