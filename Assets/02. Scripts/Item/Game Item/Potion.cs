using UnityEngine;

public class Potion : MonoBehaviour, IItem
{
    public void Use()
    {
        GameManager.Instance.Player.UpdateHP(GameManager.Instance.Player.OriginStat.HP);
    }
}
