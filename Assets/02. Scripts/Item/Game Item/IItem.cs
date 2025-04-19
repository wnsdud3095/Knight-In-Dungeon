using UnityEngine;

public interface IItem
{
    public void Use(PlayerCtrl player_ctrl);
    public void OnTriggerEnter2D(Collider2D collision);
}
