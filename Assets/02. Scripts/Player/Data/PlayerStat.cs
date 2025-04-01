using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Scriptable Objects/PlayerStat")]
public class PlayerStat : ScriptableObject
{
    [SerializeField]
    private float m_hp;
    public float HP { get { return m_hp; } set { m_hp = value; } }

    [SerializeField]
    private float m_hp_regen;
    public float HpRegen { get { return m_hp_regen; } set { m_hp_regen = value; } }

    [SerializeField]
    private float m_atk_damage;
    public float AtkDamage { get { return m_atk_damage;} set { m_atk_damage = value;} }

    [SerializeField]
    private float m_bullet_size;
    public float BulletSize { get { return m_bullet_size; } set { m_bullet_size= value; } }

    [SerializeField]
    private float m_move_speed;
    public float MoveSpeed { get { return m_move_speed;} set { m_move_speed = value; } }

    [SerializeField]
    private float m_exp_bonus_ratio;
    public float ExpBonusRatio { get { return m_exp_bonus_ratio; } set { m_exp_bonus_ratio = value; } }

    [SerializeField]
    private float m_cool_down_decrease_ratio;
    public float CoolDownDecreaseRatio { get { return m_cool_down_decrease_ratio; } set {m_cool_down_decrease_ratio= value; } }


}
