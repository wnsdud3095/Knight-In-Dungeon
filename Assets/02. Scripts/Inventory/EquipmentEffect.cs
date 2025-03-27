using UnityEngine;

[System.Serializable]
public struct EquipmentEffect
{
    [Header("추가 공격력")]
    [SerializeField] private float m_atk;
    public float ATK
    {
        get { return m_atk; }
        set { m_atk = value; }
    }

    [Header("추가 체력")]
    [SerializeField] private float m_hp;
    public float HP
    {
        get { return m_hp; }
        set { m_hp = value; }
    }

    [Header("1회 강화 시, 증가할 공격력")]
    [SerializeField] private float m_growth_atk;
    public float GrowthATK
    {
        get { return m_growth_atk; }
        set { m_growth_atk = value; }
    }

    [Header("1회 강화 시, 증가할 체력")]
    [SerializeField] private float m_growth_hp;
    public float GrowthHP
    {
        get { return m_growth_hp; }
        set { m_growth_hp = value; }
    }

    [Header("최대 강화치")]
    [SerializeField] private int m_reinforcement_limit;
    public int MaxReinforce
    {
        get { return m_reinforcement_limit; }
    }

    public static EquipmentEffect operator+(EquipmentEffect arg1, EquipmentEffect arg2)
    {
        return new EquipmentEffect
        {
            ATK = arg1.ATK + arg2.ATK,
            HP = arg1.HP + arg2.HP
        };
    }
}