using UnityEngine;
using Fusion;

public class Bomb : NetworkBehaviour, IItem
{
    [Header("폭발 이펙트")]
    [SerializeField] private GameObject m_explosion_effect;

    public void Use(PlayerCtrl player_ctrl)
    {
        Instantiate(m_explosion_effect);

        EnemyCtrl[] enemies = FindObjectsByType<EnemyCtrl>(sortMode: FindObjectsSortMode.None);

        foreach(EnemyCtrl enemy in enemies)
        {
            Vector3 view_position = Camera.main.WorldToViewportPoint(enemy.transform.position);

            if (view_position.z > 0 && view_position.x >= 0 && view_position.x <= 1 && view_position.y >= 0 && view_position.y <= 1)
            {
                enemy.Die();
            }
        }
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
