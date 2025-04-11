using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Scriptable Object/Create Enemy")]
public class Enemy : ScriptableObject
{
    [Header("몬스터의 체력")]
    [SerializeField] private float m_hp;
    public float HP
    {
        get { return m_hp; }
    }

    [Header("몬스터의 공격력")]
    [SerializeField] private float m_atk;
    public float ATK
    {
        get { return m_atk; }
    }

    [Header("몬스터의 이동속도")]
    [SerializeField] private float m_speed;
    public float SPD
    {
        get { return m_speed; }
    }

    [Header("몬스터의 경험치")]
    [SerializeField] private int m_exp;
    public int EXP
    {
        get { return m_exp; }
    }

    [Header("몬스터의 둔화저항력")]
    [SerializeField] private float m_slow_resistance;
    public float AntiSlow
    {
        get { return m_slow_resistance; }
    }

    [Header("몬스터의 넉백저항력")]
    [SerializeField] private float m_knockback_resistance;
    public float AntiKnockback
    {
        get { return m_knockback_resistance; }
    }

    [Header("몬스터의 빙결저항력")]
    [SerializeField] private float m_freeze_resistance;
    public float AntiFreeze
    {
        get { return m_freeze_resistance; }
    }

    [Header("몬스터의 애니메이터")]
    [SerializeField] private RuntimeAnimatorController m_animator;
    public RuntimeAnimatorController Animator
    {
        get { return m_animator; }
    }

    [Header("보스 여부")]
    [SerializeField] private bool m_is_boss;
    public bool Boss
    {
        get { return m_is_boss; }
    }
}