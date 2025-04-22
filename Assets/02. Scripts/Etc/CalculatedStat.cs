public class CalculatedStat
{
    private float m_hp;
    public float HP
    {
        get { return m_hp; }
        set { m_hp = value; }
    }

    private float m_atk;
    public float ATK
    {
        get { return m_atk; }
        set { m_atk = value; }
    }

    private float m_hp_regen;
    public float HP_REGEN
    {
        get { return m_hp_regen; }
        set { m_hp_regen = value; }
    }

    private int m_weapon_id;
    public int WeaponID { get { return m_weapon_id; } set { m_weapon_id = value; } }

    public CalculatedStat(float hp, float atk, float hp_regen)
    {
        HP = hp;
        ATK = atk;
        HP_REGEN = hp_regen;
    }
    public CalculatedStat(float hp, float atk, float hp_regen, int weapon_id)
    {
        HP = hp;
        ATK = atk;
        HP_REGEN = hp_regen;
        WeaponID = weapon_id;
    }
}
