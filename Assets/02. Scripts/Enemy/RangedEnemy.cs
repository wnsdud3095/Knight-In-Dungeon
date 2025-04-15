using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Enemy", menuName = "Scriptable Object/Create Ranged Enemy")]
public class RangedEnemy : Enemy
{
    [Space(50)]
    [Header("몬스터의 사거리")]
    [SerializeField] private float m_atk_range;
    public float Range => m_atk_range;

    [Header("발사물의 속도")]
    [SerializeField] private float m_projectile_speed;
    public float ProjectileSPD => m_projectile_speed;
}
