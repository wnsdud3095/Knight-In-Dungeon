using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class Skill2_ThrowAxe : PlayerSkillBase
{
    private int m_skill_id = 101;

    protected float m_skill2_damage_ratio = 1f;
    private float m_damage_level_ratio = 1f;
    private float m_damage_levelup_ratio = 0.2f;

    private float m_skill2_cool_time = 5f;
    private int m_axe_count = 1;

    private int m_axe_increase = 1;
    private float m_cool_time_decrease = 0.5f;

    private void Start()
    {
        m_cool_time = m_skill2_cool_time;
    }

    public override void UseSKill()
    {
        CoolTime(m_cool_time);

        if(m_can_use)
        {
            SpawnAxe();
        }
    }

    protected virtual void SpawnAxe()
    {
        SoundManager.Instance.PlayEffect("Axe SFX");

        for(int i = 0; i < m_axe_count; i++)
        {
            Vector2 throw_direction = Vector2.up;

            float angle = Random.Range(-20f, 20f);

            Vector2 final_thorw_direction = Quaternion.Euler(0, 0, angle) * throw_direction;

            float throw_force = Random.Range(7f, 10f);

            GameObject prefab = GameManager.Instance.BulletPool.Get(SkillBullet.Axe);
            prefab.transform.position = (Vector2)GameManager.Instance.Player.transform.position + Vector2.up * 0.25f;

            float rotation_angle = Random.Range(0f, 360f);
            prefab.transform.rotation = Quaternion.Euler(0, 0, rotation_angle);
            
            var axe = prefab.GetComponent<Axe>();
            axe.Damage = GetFinallDamage(m_skill2_damage_ratio, m_damage_level_ratio);
            axe.Rigidbody.AddForce(final_thorw_direction * throw_force, ForceMode2D.Impulse);
        }
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        m_damage_level_ratio += m_damage_levelup_ratio;
        m_axe_count += m_axe_increase;
        m_cool_time -= m_cool_time_decrease;
    }
}
