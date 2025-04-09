using UnityEngine;

public class ESkill5_MagicMissile : Skill5_MagicMissile
{
    private float m_e_skill5_cool_time = 1f;
    private float m_e_throw_speed = 8f;
    private float m_damage_e_level_ratio = 2f; // 스킬 만랩의 레벨별 공격력 배수

    new void Awake()
    {
        m_cool_time = m_e_skill5_cool_time;
    }

    protected override void SpawnMissile()
    {
        int rand_missile = Random.Range((int)SkillBullet.FireBall,(int)SkillBullet.IceBolt + 1 );

        var prefab = GameManager.Instance.BulletPool.Get((SkillBullet)rand_missile);
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

        prefab.GetComponent<MagicMissile>().Damage = GetFinallDamage(m_skill5_damage_ratio, m_damage_e_level_ratio);
        prefab.GetComponent<MagicMissile>().Speed = m_e_throw_speed;
    }
}
