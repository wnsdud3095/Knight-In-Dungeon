using UnityEngine;

public class ESkill6_PiercingLight : Skill6_PiercingLight
{
    private float m_e_skill6_cool_time = 3f;
    private int m_h_light_count = 5;
    private int m_v_light_count = 3;

    private float m_v_light_expand = 6.2f;

    private float m_damage_e_level_ratio = 2f; // 스킬 만랩의 레벨별 공격력 배수

    protected override void Awake()
    {
        base.Awake();
        m_cool_time = m_e_skill6_cool_time;
        m_light_count = m_h_light_count;
    }

    protected override  void SpawnLight()
    {
        base.SpawnLight();
        for (int i = 0; i < m_v_light_count; i++)
        {
            var prefab = GameManager.Instance.BulletPool.Get(SkillBullet.PiercingLight);
            prefab.transform.SetParent(GameManager.Instance.Player.transform);
            prefab.transform.rotation = Quaternion.Euler(0f, 0f, -90f);

            float spacing = m_cam_width / (m_v_light_count + 1);
            float x = m_cam.transform.position.x - m_cam_width / 2 + spacing * (i + 1);
            float y = m_cam.transform.position.y + m_cam_height / 2f;

            prefab.transform.position = new Vector2(x, y);
            prefab.transform.localScale = Vector3.one * GameManager.Instance.Player.Stat.BulletSize;

            prefab.GetComponent<PiercingLight>().Damage = GetFinallDamage(m_skill6_damage_ratio, m_damage_e_level_ratio);
            prefab.GetComponent<PiercingLight>().LightExpand = m_v_light_expand;
        }
    }
}
