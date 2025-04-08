using UnityEngine;

public class ESkill1_KunaiThorw : Skill1_KunaiThorw
{
    private float m_e_skill_cool_time = 2f;
    private int m_e_kunai_count = 25;
    private float m_damage_e_level_ratio = 2f; // 스킬 만랩의 레벨별 공격력 배수
    private int m_e_reflect_count = 2;


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

            prefab.GetComponent<Kunai>().Damage = GetFinallDamage(m_skill1_damage_ratio,m_damage_e_level_ratio);
            prefab.GetComponent<Kunai>().ReflectCount = m_e_reflect_count;

        }
    }
}
