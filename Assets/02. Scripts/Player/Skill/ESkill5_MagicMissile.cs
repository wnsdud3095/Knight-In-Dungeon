using UnityEngine;

public class ESkill5_MagicMissile : Skill5_MagicMissile
{
    private float m_e_skill5_cool_time = 1f;
    private float m_e_throw_speed = 8f;
    private float m_e_damage;

    void Awake()
    {
        m_cool_time = m_e_skill5_cool_time;
        m_e_damage = GameManager.Instance.Player.Stat.AtkDamage * 4f;
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
        prefab.GetComponent<MagicMissile>().Damage = m_e_damage;
        prefab.GetComponent<MagicMissile>().Speed = m_e_throw_speed;
    }
}
