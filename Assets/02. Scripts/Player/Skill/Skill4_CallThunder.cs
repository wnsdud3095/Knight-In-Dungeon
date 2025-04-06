using UnityEngine;
using System.Collections.Generic;

public class Skill4_CallThunder : PlayerSkillBase
{
    private float m_skill4_cool_time = 5f;
    protected int m_thunder_count = 1;

    private int m_thunder_increase = 1;
    private float m_cool_time_decrease = 1f;

    private float m_damage_up_ratio = 1.2f;
    protected float m_damage;

    private ScreenOutlinCtrl m_screen;

    protected SkillBullet m_bullet;

    protected virtual void Start()
    {
        m_bullet = SkillBullet.Thunder;
        m_cool_time = m_skill4_cool_time;
        m_screen = GameObject.FindAnyObjectByType<ScreenOutlinCtrl>();
        m_damage = GameManager.Instance.Player.Stat.AtkDamage * 1.5f;
    }


    public override void UseSKill()
    {
        CoolTime(m_cool_time);

        if(m_can_use)
        {
            SpawnThunder();
        }
    }

    public void SpawnThunder()
    {
        Collider2D[] m_cols = Physics2D.OverlapBoxAll(m_screen.Cam.transform.position, new Vector2(m_screen.CamWidth, m_screen.CamHeight),0);

        List<int> rand_enemy_indexs = new List<int>();

        for (int i = 0; i < m_cols.Length; i++)
        {
            if (m_cols[i].CompareTag("Enemy"))
            {
                rand_enemy_indexs.Add(i);
            }
        }
        if(rand_enemy_indexs.Count == 0)
        {
            Debug.Log("적이 범위 내에 없음");
            return;
        }
        Shuffle(rand_enemy_indexs);


        for (int i = 0; i < m_thunder_count; i++)
        {
            var prefab = GameManager.Instance.BulletPool.Get(m_bullet);
            prefab.transform.SetParent(GameManager.Instance.BulletPool.transform);
            prefab.transform.position = m_cols[rand_enemy_indexs[i % rand_enemy_indexs.Count]].transform.position;

            prefab.GetComponent<Thunder>().Damage = m_damage;
            prefab.SetActive(true);
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
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
            m_thunder_count += m_thunder_increase;
        }
    }
}
