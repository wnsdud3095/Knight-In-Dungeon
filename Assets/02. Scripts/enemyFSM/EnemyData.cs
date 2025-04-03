using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data", order = 1)]
public class EnemyData : ScriptableObject
{
    public float moveSpeed = 2f;      // 이동 속도
    public float attackRange = 1.5f;  // 공격 범위
    public int maxHealth = 100;       // 최대 체력
    public int Exp = 10;              // 경험치
}
