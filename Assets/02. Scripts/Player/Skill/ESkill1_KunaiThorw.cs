using UnityEngine;

public class ESkill1_KunaiThorw : Skill1_KunaiThorw
{
    private float m_e_skill_cool_time = 2f;
    private int m_e_kunai_count = 25;
    private float m_e_damage;
    private int m_e_reflect_count = 2;

    void Start()
    {
        m_e_damage = GameManager.Instance.Player.Stat.AtkDamage * 2.2f;
    }

    public override void UseSKill()
    {
        CoolTime(m_e_skill_cool_time);


        if (m_can_use)
        {
            SpawnKunai();
        }
    }

    protected override void SpawnKunai()
    {
        for (int i = 0; i < m_e_kunai_count; i++)
        {
            var prefab = GameManager.Instance.BulletPool.Get(SkillBullet.Kunai);
            prefab.transform.SetParent(GameManager.Instance.BulletPool.transform);
            prefab.transform.position = GameManager.Instance.Player.transform.position;

            Vector3 rotate_vec = Vector3.forward * 360 * i / m_e_kunai_count;
            prefab.transform.Rotate(rotate_vec);
            prefab.transform.Translate(Vector3.up);

            prefab.GetComponent<Kunai>().Damage = m_e_damage;
            prefab.GetComponent<Kunai>().ReflectCount = m_e_reflect_count;
            prefab.GetComponent<Kunai>().LifeTime = m_kunai_life_time;

        }
    }
}
