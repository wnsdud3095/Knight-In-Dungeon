using UnityEngine;

public class Skill5_MagicMissile : PlayerSkillBase
{
    private int m_skill_id = 4;
    //데미지 관련
    protected float m_skill5_damage_ratio = 1.1f; // 스킬의 공격력 계수
    protected float m_damage_level_ratio = 1f; // 레벨별 공격력 배수
    protected float m_damage_levelup_ratio = 0.2f; //레벨업시 공격력 배수가 증가하는 수치

    private float m_skill5_cool_time = 1f;
    protected float m_throw_speed = 6f;

    private float m_throw_speed_increase = 2f;
    protected float m_cool_time_decrease = 0.25f;

    private float m_detect_radius = 3.5f;

    private Collider2D[] cols;

    protected Transform m_nearest_target;

    protected SkillBullet m_bullet;
    protected string m_sfx_name;

    protected override void Awake()
    {
        base.Awake();
        m_cool_time = m_skill5_cool_time;
        m_bullet = SkillBullet.MagicMissile;
        m_sfx_name = "Magic Bullet SFX";
    }

    public override void UseSKill()
    {
        cols = Physics2D.OverlapCircleAll(GameManager.Instance.Player.transform.position, m_detect_radius);
        m_nearest_target = GetNearest();

        if (m_nearest_target == null || m_nearest_target.gameObject.activeSelf == false)
        {
            return;
        }

        CoolTime(m_cool_time);

        if(m_can_use)
        {

            SpawnMissile(m_bullet);
        }
    }

    protected Transform GetNearest()
    {
        Transform result = null;
        float min = 100f;

        foreach(Collider2D col in cols)
        {
            if (col.CompareTag("Enemy"))
            {
                Vector3 player_pos = GameManager.Instance.Player.transform.position;
                Vector3 enemy_pos = col.transform.position;
                float diff = Vector3.Distance(player_pos, enemy_pos);

                if (min > diff)
                {
                    min = diff;
                    result = col.transform;
                }
            }
        }
        return result;
    }

    protected virtual void SpawnMissile(SkillBullet bullet)
    {
        SoundManager.Instance.PlayEffect(m_sfx_name);

        var prefab = GameManager.Instance.BulletPool.Get(bullet);
        prefab.transform.SetParent(GameManager.Instance.BulletPool.transform);
        prefab.transform.position = GameManager.Instance.Player.transform.position;

        Vector3 dir;

        if (m_nearest_target == null)
        {
            dir = Vector3.zero;
        }
        else
        {
            dir = (m_nearest_target.position - GameManager.Instance.Player.transform.position).normalized; //적과 플레이어 사이의 방향 벡터를 정규화
        }
        
        prefab.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        prefab.transform.localScale = Vector3.one * GameManager.Instance.Player.Stat.BulletSize;

        prefab.GetComponent<MagicMissile>().Damage = GetFinallDamage(m_skill5_damage_ratio, m_damage_level_ratio);
        prefab.GetComponent<MagicMissile>().Speed = m_throw_speed;
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        CheckSkillEvolve(m_skill_id);
        m_damage_level_ratio += m_damage_levelup_ratio;
        if (level%2 == 0)
        {
            m_cool_time -= m_cool_time_decrease;
        }
        else
        {
            m_throw_speed += m_throw_speed_increase;
        }
    }
}
