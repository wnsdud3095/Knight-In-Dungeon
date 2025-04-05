using UnityEngine;

public class Bomb : MonoBehaviour, IItem
{
    public void Use()
    {
        EnemyFSM[] enemies = FindObjectsByType<EnemyFSM>(sortMode: FindObjectsSortMode.None);

        foreach(EnemyFSM enemy in enemies)
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
            Use();
            ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Item_Bomb);
        }
    }
}
