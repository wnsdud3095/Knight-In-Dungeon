using UnityEngine;

public class Skill1_HolySword : PlayerSkillBase
{
    private int m_skill_id = 100;

    protected float m_skill1_damage_ratio = 1.25f;
    private float m_damage_level_ratio = 1f;
    private float m_damage_levelup_ratio = 0.2f;

    private float m_skill1_cool_time = 4f;
    private int m_sword_count = 1;

    private int m_sword_increase = 1;
    private float m_cool_time_decrease = 0.5f;

    private Vector2 m_save_input_vector = Vector2.right;

    protected Transform m_container;

    private void Start()
    {
        m_cool_time = m_skill1_cool_time;

       Transform[] transforms = GameManager.Instance.Player.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform container in transforms)
        {
            if (container.name == "Holy Sword Container")
            {
                m_container = container;
            }
        }        
    }

    public override void UseSKill()
    {
        CoolTime(m_cool_time);

        SaveInputVector();

        if(m_can_use)
        {
            SpawnHolySword();
        }
    }

    private void SaveInputVector()
    {
        if(GameManager.Instance.Player.joyStick.GetInputVector() != Vector2.zero)
        {
            m_save_input_vector = GameManager.Instance.Player.joyStick.GetInputVector();
        }
    }

    protected virtual void SpawnHolySword()
    {
        if(m_save_input_vector == Vector2.zero)
        {
            return;
        }

        float total_angle = 360f;
        float start_angle = -total_angle / 2f;
        float angle_step = (m_sword_count > 1) ? total_angle / m_sword_count : 0f;

        for(int i = 0; i < m_sword_count; i++)
        {
            float angle = start_angle + angle_step * i;

            if(m_sword_count == 1)
            {
                angle = 0f;
            }

            Vector2 rotated_direction = Quaternion.Euler(0, 0, angle) * m_save_input_vector;

            var prefab = GameManager.Instance.BulletPool.Get(SkillBullet.HolySword);
            prefab.transform.SetParent(m_container);
            prefab.transform.position = GameManager.Instance.Player.transform.position;
            prefab.transform.rotation = Quaternion.LookRotation(Vector3.forward, rotated_direction);

            prefab.transform.Translate(prefab.transform.up * 0.25f, Space.World);

            var holy_sword = prefab.GetComponent<HolySword>();
            holy_sword.Damage = GetFinallDamage(m_skill1_damage_ratio, m_damage_level_ratio);
        }
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        m_damage_level_ratio += m_damage_levelup_ratio;
        
        m_sword_count += m_sword_increase;
        m_cool_time -= m_cool_time_decrease;
    }
}
