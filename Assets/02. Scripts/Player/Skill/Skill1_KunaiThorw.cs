using UnityEngine;

public class Skill1_KunaiThorw : PlayerSkillBase
{
    [SerializeField]
    private float m_skill1_cool_time = 4f;
    private int m_kunal_count = 1;
    private float m_damage;
    private int m_reflect_count = 1;

    private float m_spawn_up_area_min = 0.2f;
    private float m_spawn_up_area_max = 1f;

    private float m_spawn_right_area_max = 0.7f;
   
    private float m_damage_up_ratio = 1.2f;



    private Vector2 save_input_vector = Vector2.right;


    void Start()
    {
        m_cool_time = m_skill1_cool_time;
        m_damage = GameManager.Instance.Player.Stat.AtkDamage;
    }

    public override void UseSKill()
    {
        CoolTime(m_skill1_cool_time);

        SaveInputVector();

        if (m_can_use)
        {
            SpawnKunai();
        }
    }

    private void SaveInputVector()
    {
        if (GameManager.Instance.Player.joyStick.GetInputVector() != Vector2.zero)
        {
            save_input_vector = GameManager.Instance.Player.joyStick.GetInputVector();
        }
    }

    protected virtual void SpawnKunai()
    {
        for (int i = 0; i < m_kunal_count; i++)
        {
            var prefab = GameManager.Instance.BulletPool.Get(SkillName.KunaiThrow);
            prefab.transform.SetParent(GameManager.Instance.BulletPool.transform);
            prefab.transform.position = GameManager.Instance.Player.transform.position;

            prefab.transform.rotation = Quaternion.LookRotation(Vector3.forward, save_input_vector); //z축을 기준으로 벡터 방향을 바라보게 회전 시킴
            prefab.transform.Translate(Vector3.up * Random.Range(m_spawn_up_area_min, m_spawn_up_area_max));
            prefab.transform.Translate(Vector3.right * Random.Range(-m_spawn_right_area_max, m_spawn_right_area_max));
            prefab.GetComponent<Kunai>().SetDamage(m_damage);
            prefab.GetComponent<Kunai>().SetReflectCount(m_reflect_count);
            //prefab.GetComponent<Kunai>().SetLifeTime();
        }   
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        m_damage *= m_damage_up_ratio;
        if(level%2 == 0)
        {
            m_kunal_count += 2;
        }
        else
        {
            m_skill1_cool_time -= 1f;
        }

    }
}
