using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill3_HolyWand : PlayerSkillBase
{
    private int m_skill_id = 101;

    protected float m_skill3_damage_ratio = 1f;
    private float m_damage_level_ratio = 1f;
    private float m_damage_levelup_ratio = 0.2f;

    private float m_skill3_cool_time = 5f;

    protected int m_sphere_count = 1;

    private int m_sphere_increase = 1;
    private float m_cool_time_decrease = 0.5f;

    private ScreenOutlinCtrl m_screen;

    protected SkillBullet m_bullet;

    protected virtual void Start()
    {
        m_bullet = SkillBullet.MagicSphere;
        m_cool_time = m_skill3_cool_time;
        m_screen = FindAnyObjectByType<ScreenOutlinCtrl>();
    }

    public override void UseSKill()
    {
        CoolTime(m_cool_time);

        if(m_can_use)
        {
            SpawnMagicSphere();
        }
    }

    protected virtual void SpawnMagicSphere()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(m_screen.Cam.transform.position, new Vector2(m_screen.CamWidth, m_screen.CamHeight), 0);

        List<int> rand_enemy_indexs = new List<int>();

        for(int i = 0; i < colliders.Length; i++)
        {
            rand_enemy_indexs.Add(i);
        }

        if(rand_enemy_indexs.Count == 0)
        {
            return;
        }

        Shuffle(rand_enemy_indexs);

        for(int i = 0; i < m_sphere_count; i++)
        {
            var prefab = GameManager.Instance.BulletPool.Get(m_bullet);
            prefab.transform.SetParent(GameManager.Instance.BulletPool.transform);
            prefab.transform.position = colliders[rand_enemy_indexs[i % rand_enemy_indexs.Count]].transform.position;

            prefab.GetComponent<MagicSphere>().Damage = GetFinallDamage(m_skill3_damage_ratio, m_damage_level_ratio);

            prefab.SetActive(true);
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        m_damage_level_ratio += m_damage_levelup_ratio;

        m_cool_time_decrease -= m_cool_time_decrease;
        m_sphere_count += m_sphere_increase;
    }
}
