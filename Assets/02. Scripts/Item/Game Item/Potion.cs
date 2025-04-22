using UnityEngine;

public class Potion : MonoBehaviour, IItem
{
    public void Use()
    {
        SoundManager.Instance.PlayEffect("Potion SFX");
        GameManager.Instance.Player.UpdateHP(GameManager.Instance.Player.OriginStat.HP);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Use();
            ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Item_Potion);
        }
    }
}
