using UnityEngine;

public class Skill5_MagicMissile : PlayerSkillBase
{
    private float m_skill5_cool_time = 1f;
    private float m_throw_speed = 6f;

    private float m_throw_speed_increase = 2f;
    private float m_cool_time_decrease = 0.25f;

    private float m_damage;

    private float m_damage_up_ratio = 1.2f;

    private float m_detect_radius = 4f;

    private Collider2D[] cols;

    private Transform m_nearest_target;

    private void Awake()
    {
        m_cool_time = m_skill5_cool_time;
        m_damage = GameManager.Instance.Player.Stat.AtkDamage *2;
    }



    public override void UseSKill()
    {
        cols = Physics2D.OverlapCircleAll(GameManager.Instance.Player.transform.position, m_detect_radius);
        m_nearest_target = GetNearest();

        CoolTime(m_cool_time);

        if(m_can_use)
        {
            SpawnMissile();
        }
    }

    private Transform GetNearest()
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

    protected virtual void SpawnMissile()
    {
        var prefab = GameManager.Instance.BulletPool.Get(SkillBullet.MagicMissile);
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
        prefab.GetComponent<MagicMissile>().Damage = m_damage;
        prefab.GetComponent<MagicMissile>().Speed = m_throw_speed;
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        m_damage *= m_damage_up_ratio;
        if(level%2 == 0)
        {
            m_cool_time -= m_cool_time_decrease;
        }
        else
        {
            m_throw_speed += m_throw_speed_increase;
        }
    }
}
