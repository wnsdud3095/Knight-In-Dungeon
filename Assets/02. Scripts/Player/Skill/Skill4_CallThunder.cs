using UnityEngine;
using System.Collections.Generic;

public class Skill4_CallThunder : PlayerSkillBase
{
    private float m_skill4_cool_time = 3f;
    private int m_thunder_count = 3;

    private int m_thunder_increase = 1;
    private float m_cool_time_decrease = 0.7f;

    private float m_damage_up_ratio = 1.2f;
    private float m_damage;

    private ScreenOutlinCtrl m_screen;

    void Start()
    {
        m_cool_time = m_skill4_cool_time;
        m_screen = GameObject.FindAnyObjectByType<ScreenOutlinCtrl>();
        m_damage = GameManager.Instance.Player.Stat.AtkDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
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

        for(int i =0; i < rand_enemy_indexs.Count; i++)
        {
            Debug.Log($"랜덤 적 리스트 {i} : {m_cols[rand_enemy_indexs[i]].name}");
        }

        for (int i = 0; i < m_thunder_count; i++)
        {
            var prefab = GameManager.Instance.BulletPool.Get(SkillBullet.Thunder);
            prefab.transform.SetParent(GameManager.Instance.BulletPool.transform);
            prefab.transform.position = m_cols[rand_enemy_indexs[i % rand_enemy_indexs.Count]].transform.position;
            Debug.Log($"번개 쏘는 인덱스 {rand_enemy_indexs[i % rand_enemy_indexs.Count] }");
            Debug.Log($"{m_cols[rand_enemy_indexs[i % rand_enemy_indexs.Count]].name} 번개 쏨");
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(m_screen.Cam.transform.position, new Vector2(m_screen.CamWidth, m_screen.CamHeight));
    }
}
