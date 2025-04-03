using UnityEngine;

public class Skill3_SpinningShuriken : PlayerSkillBase
{
    private float m_skill3_cool_time = 5f;
    private int m_shuriken_count = 2;

    private int m_shuriken_increase = 2;

    private float m_spinning_spped = 150f;
    private float m_spinning_radius = 1f;
    private float m_spinning_spped_up_ratio = 1.2f;
    private float m_spinning_radius_increase = 1.5f;

    private float m_life_time = 5f;

    private float m_damage;
    private float m_damage_up_ratio = 1.2f;

    protected GameObject m_rotater;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_cool_time = m_skill3_cool_time;
        m_damage = GameManager.Instance.Player.Stat.AtkDamage;

        Transform[] transforms = GameManager.Instance.Player.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in transforms)
        {
            if (t.gameObject.name == "ShurikenRotater")
            {
                m_rotater = t.gameObject;
            }
        }
    }


    public override void UseSKill()
    {
        CoolTime(m_cool_time + m_life_time);

        if(m_can_use)
        {
            SpawnShuriken();
        }
    }

    private void SpawnShuriken()
    {
        for(int i = 0; i<m_shuriken_count; i++)
        {
            var prefab = GameManager.Instance.BulletPool.Get(SkillBullet.Shuriken);
            prefab.transform.SetParent(m_rotater.transform);
            prefab.transform.position = GameManager.Instance.Player.transform.position;
            prefab.transform.rotation = Quaternion.Euler(Vector3.zero);

            Vector3 rot_vec = Vector3.forward * 360 * i / m_shuriken_count;
            prefab.transform.Rotate(rot_vec);
            prefab.transform.Translate(prefab.transform.up * m_spinning_radius , Space.World);
            prefab.GetComponent<Shuriken>().Damage = m_damage;
            prefab.GetComponent<Shuriken>().LifeTime = m_life_time;
            m_rotater.SetActive(true);
            m_rotater.GetComponent<ShurikenRotater>().SpinningSpeed = m_spinning_spped;
        }
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        m_damage *= m_damage_up_ratio;
        if(level % 2 ==0)
        {
            m_spinning_spped *= m_spinning_spped_up_ratio;
        }
        else
        {
            m_shuriken_count += m_shuriken_increase;
            m_spinning_radius += m_spinning_radius_increase;
        }

    }
}
