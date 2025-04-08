using UnityEngine;
using System.Collections.Generic;

public class Skill4_CallThunder : PlayerSkillBase
{
    //데미지 관련
    protected float m_skill4_damage_ratio = 1.3f; // 스킬의 공격력 계수
    protected float m_damage_level_ratio = 1f; // 레벨별 공격력 배수
    private float m_damage_levelup_ratio = 0.2f; //레벨업시 공격력 배수가 증가하는 수치

    private float m_skill4_cool_time = 5f;
    protected int m_thunder_count = 1;

    private int m_thunder_increase = 1;
    private float m_cool_time_decrease = 1f;

    private ScreenOutlinCtrl m_screen;

    protected SkillBullet m_bullet;

    protected virtual void Start()
    {
        m_bullet = SkillBullet.Thunder;
        m_cool_time = m_skill4_cool_time;
        m_screen = GameObject.FindAnyObjectByType<ScreenOutlinCtrl>();
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
            prefab.transform.localScale = Vector3.one * GameManager.Instance.Player.Stat.BulletSize;

            prefab.GetComponent<Thunder>().Damage = GetFinallDamage(m_skill4_damage_ratio, m_damage_level_ratio);

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
        m_damage_level_ratio += m_damage_levelup_ratio;
        if (level%2 == 0)
        {
            m_cool_time -= m_cool_time_decrease;
        }
        else
        {
            m_thunder_count += m_thunder_increase;
        }
    }
}
