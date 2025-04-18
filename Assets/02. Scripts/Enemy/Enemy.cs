using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Scriptable Object/Create Enemy")]
public class Enemy : ScriptableObject
{
    [Header("몬스터의 고유한 ID")]
    [SerializeField] private int m_id;
    public int ID
    {
        get { return m_id; }
    }
    
    [Header("몬스터의 체력")]
    [SerializeField] private float m_hp;
    public float HP
    {
        get { return m_hp; }
    }

    [Header("몬스터의 공격력")]
    [SerializeField] private float m_atk;
    public float ATK => m_atk;

    [Header("몬스터의 이동속도")]
    [SerializeField] private float m_speed;
    public float SPD => m_speed;

    [Header("몬스터의 경험치")]
    [SerializeField] private int m_exp;
    public int EXP => m_exp;

    [Header("몬스터의 둔화저항력")]
    [SerializeField] private float m_slow_resistance;
    public float AntiSlow => m_slow_resistance;

    [Header("몬스터의 넉백저항력")]
    [SerializeField] private float m_knockback_resistance;
    public float AntiKnockback => m_knockback_resistance;

    [Header("몬스터의 빙결저항력")]
    [SerializeField] private float m_freeze_resistance;
    public float AntiFreeze => m_freeze_resistance;

    [Header("몬스터의 애니메이터")]
    [SerializeField] private RuntimeAnimatorController m_animator;
    public RuntimeAnimatorController Animator => m_animator;

    [Header("몬스터의 타입")]
    [SerializeField] private EnemyType m_enemy_type;
    public EnemyType EnemyType => m_enemy_type;

    [Header("보스 여부")]
    [SerializeField] private bool m_is_boss;
    public bool Boss => m_is_boss;
}