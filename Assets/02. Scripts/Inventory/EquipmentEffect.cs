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

    public static EquipmentEffect operator+(EquipmentEffect arg1, EquipmentEffect arg2)
    {
        return new EquipmentEffect
        {
            ATK = arg1.ATK + arg2.ATK,
            HP = arg1.HP + arg2.HP
        };
    }

    public static EquipmentEffect operator*(EquipmentEffect arg1, int arg2)
    {
        return new EquipmentEffect
        {
            ATK = arg1.ATK * arg2,
            HP = arg1.HP * arg2
        };
    }
}