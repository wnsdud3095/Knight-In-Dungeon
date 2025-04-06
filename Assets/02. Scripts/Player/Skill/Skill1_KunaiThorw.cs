using UnityEngine;

public class Skill1_KunaiThorw : PlayerSkillBase
{
    [SerializeField]
    private float m_skill1_cool_time = 4f;
    private int m_kunal_count = 1;

    private int m_kunai_increase = 2;
    private float m_cool_time_decrease = 1;

    private float m_damage;
    private int m_reflect_count = 1;

    private float m_spawn_up_area_min = 0.2f;
    private float m_spawn_up_area_max = 0.6f;

    private float m_spawn_right_area_max = 0.5f;
   
    private float m_damage_up_ratio = 1.2f;

    private Vector2 save_input_vector = Vector2.right;


    void Start()
    {
        m_cool_time = m_skill1_cool_time;
        m_damage = GameManager.Instance.Player.Stat.AtkDamage;
    }

    public override void UseSKill()
    {
        CoolTime(m_cool_time);

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
            var prefab = GameManager.Instance.BulletPool.Get(SkillBullet.Kunai);
            prefab.transform.SetParent(GameManager.Instance.BulletPool.transform);
            prefab.transform.position = GameManager.Instance.Player.transform.position;

            prefab.transform.rotation = Quaternion.LookRotation(Vector3.forward, save_input_vector); //z축을 기준으로 벡터 방향을 바라보게 회전 시킴
            prefab.transform.Translate(Vector3.up * Random.Range(m_spawn_up_area_min, m_spawn_up_area_max));
            prefab.transform.Translate(Vector3.right * Random.Range(-m_spawn_right_area_max, m_spawn_right_area_max));
            prefab.GetComponent<Kunai>().Damage = m_damage; //효율적인 참조를 위해 Get 말고 Set 방식 사용
            prefab.GetComponent<Kunai>().ReflectCount = m_reflect_count;
        }   
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        m_damage *= m_damage_up_ratio;
        if(level%2 == 0)
        {
            m_kunal_count += m_kunai_increase;
        }
        else
        {
            m_cool_time -= m_cool_time_decrease;
        }

    }
}
