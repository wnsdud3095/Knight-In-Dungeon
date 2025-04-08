using UnityEngine;

public class Skill6_PiercingLight : PlayerSkillBase
{
    private float m_skill6_cool_time = 5f;
    protected int m_light_count = 2;

    private int m_light_increase = 1;
    private float m_cool_time_decrease = 1;

    protected float m_damage;

    protected Camera m_cam;
    protected float m_cam_height;
    protected float m_cam_width;

    private float m_damage_up_ratio = 1.4f;

    private float m_light_expand = 3.2f;

    protected virtual void Awake()
    {
        m_cam = Camera.main;
        m_cam_height = m_cam.orthographicSize * 2f;
        m_cam_width = m_cam_height * m_cam.aspect;

        m_cool_time = m_skill6_cool_time;
        m_damage = GameManager.Instance.Player.Stat.AtkDamage;
    }

    public override void UseSKill()
    {
        CoolTime(m_cool_time);
        if(m_can_use)
        {
            SpawnLight();
        }
    }

    protected virtual void SpawnLight()
    {
        for (int i =0; i < m_light_count; i++)
        {
            var prefab = GameManager.Instance.BulletPool.Get(SkillBullet.PiercingLight);
            prefab.transform.SetParent(GameManager.Instance.Player.transform);

            float x = m_cam.transform.position.x -m_cam_width / 2; // 카메라 기준으로 좌측 끝을 계산

            float spacing = m_cam_height / (m_light_count + 1);
            float y = m_cam.transform.position.y - m_cam_height / 2f + spacing * (i + 1); //카메라 기준으로 y 좌표 계산

            prefab.transform.position = new Vector2(x, y);
            prefab.GetComponent<PiercingLight>().Damage= m_damage;
            prefab.GetComponent<PiercingLight>().LightExpand = m_light_expand;
        }
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        m_damage *= m_damage_up_ratio;
        if (level % 2 == 0)
        {
            m_light_count += m_light_increase;
        }
        else
        {
            m_cool_time -= m_cool_time_decrease;
        }
    }
}
