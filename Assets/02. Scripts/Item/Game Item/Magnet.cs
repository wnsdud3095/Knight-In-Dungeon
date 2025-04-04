using UnityEngine;

public class Magnet : MonoBehaviour, IItem
{
    public void Use()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Use();
            ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Item_Magnet);
        }
    }
}
