using Fusion;
using UnityEngine;

public class Magnet : NetworkBehaviour, IItem
{
    public void Use(PlayerCtrl player_ctrl)
    {
        GameObject[] m_exp_orbs = ObjectManager.Instance.GetActiveObjects(ObjectType.Exp);
        
        foreach(GameObject exp in m_exp_orbs)
        {
            exp.GetComponent<Exp>().Magneted = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //Use();
            ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Item_Magnet);
        }
    }
}
