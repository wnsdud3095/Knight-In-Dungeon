using UnityEngine;

public class Skill3_SpinningShuriken : PlayerSkillBase
{
    private int m_skill_id = 2;
    //데미지 관련
    protected float m_skill3_damage_ratio = 1.0f; // 스킬의 공격력 계수
    protected float m_damage_level_ratio = 1f; // 레벨별 공격력 배수
    private float m_damage_levelup_ratio = 0.2f; //레벨업시 공격력 배수가 증가하는 수치

    protected float m_skill3_cool_time = 5f;
    protected int m_shuriken_count = 2;

    private int m_shuriken_increase = 2;

    protected float m_spinning_spped = 150f;
    protected float m_spinning_radius = 1f;
    private float m_spinning_spped_up_ratio = 1.2f;
    private float m_spinning_radius_increase = 0.3f;

    protected float m_life_time = 5f;

    protected GameObject m_rotater;


    protected virtual void Start()
    {
        m_cool_time = m_skill3_cool_time;


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
        if(m_can_use)
        {
            SpawnShuriken();
        }
        CoolTime(m_cool_time + m_life_time);
    }

    protected void SpawnShuriken()
    {
        if (!m_rotater)
        {
            Transform[] transforms = GameManager.Instance.Player.transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in transforms)
            {
                if (t.gameObject.name == "ShurikenRotater")
                {
                    m_rotater = t.gameObject;
                }
            }
        }
        for(int i = 0; i<m_shuriken_count; i++)
        {
            var prefab = GameManager.Instance.BulletPool.Get(SkillBullet.Shuriken);
            prefab.transform.SetParent(m_rotater.transform);
            prefab.transform.position = GameManager.Instance.Player.transform.position;
            prefab.transform.rotation = Quaternion.Euler(Vector3.zero);

            Vector3 rot_vec = Vector3.forward * 360 * i / m_shuriken_count;
            prefab.transform.Rotate(rot_vec);
            prefab.transform.Translate(prefab.transform.up * m_spinning_radius , Space.World);
            prefab.transform.localScale = Vector3.one * GameManager.Instance.Player.Stat.BulletSize;

            prefab.GetComponent<Shuriken>().Damage = GetFinallDamage(m_skill3_damage_ratio, m_damage_level_ratio);
            m_rotater.GetComponent<ShurikenRotater>().LifeTime = m_life_time;
            m_rotater.SetActive(true);
            m_rotater.GetComponent<ShurikenRotater>().SpinningSpeed = m_spinning_spped;
        }
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        CheckSkillEvolve(m_skill_id);
        m_damage_level_ratio += m_damage_levelup_ratio;
        if (level % 2 ==0)
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
