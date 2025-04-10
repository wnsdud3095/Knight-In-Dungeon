using UnityEngine;

public class MoneyBag : MonoBehaviour, IItem
{
    public void Use()
    {
        GameObject.Find("Stage Manager").GetComponent<StageManager>().Kill += UnityEngine.Random.Range(100, 500);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Use();
            ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Item_MoneyBag);
        }
    }
}