using UnityEngine;

public class Skill1_KunaiThorw : PlayerSkillBase
{
    private int m_skill_id = 0;
    //데미지 관련
    protected float m_skill1_damage_ratio = 1f; // 스킬의 공격력 계수
    private float m_damage_level_ratio = 1f; // 레벨별 공격력 배수
    private float m_damage_levelup_ratio = 0.2f; //레벨업시 공격력 배수가 증가하는 수치

    private float m_skill1_cool_time = 4f;
    private int m_kunal_count = 1;

    private int m_kunai_increase = 2;
    private float m_cool_time_decrease = 1;

    private int m_reflect_count = 1;   

    private Vector2 save_input_vector = Vector2.right;


    void Start()
    {
        m_cool_time = m_skill1_cool_time;
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
        if (save_input_vector == Vector2.zero) return;

        float total_angle = 60f; // 전체 부채꼴 각도
        float start_angle = -total_angle / 2f;
        float angle_step = (m_kunal_count > 1) ? total_angle / (m_kunal_count - 1) : 0f;

        SoundManager.Instance.PlayEffect("Kunai SFX");

        for (int i = 0; i < m_kunal_count; i++)
        {
            float angle = start_angle + angle_step * i;

            if(m_kunal_count == 1) angle = 0f; //1개면 -30 도 방향으로 움직임

            // 중심 방향에서 각도만큼 회전된 방향 구하기
            Vector2 rotated_dir = Quaternion.Euler(0, 0, angle) * save_input_vector.normalized;

            var prefab = GameManager.Instance.BulletPool.Get(SkillBullet.Kunai);
            prefab.transform.SetParent(GameManager.Instance.BulletPool.transform);
            prefab.transform.position = GameManager.Instance.Player.transform.position;

            // 방향에 따라 회전 적용
            prefab.transform.rotation = Quaternion.LookRotation(Vector3.forward, rotated_dir);

            prefab.transform.localScale = Vector3.one * GameManager.Instance.Player.Stat.BulletSize;

            var kunai = prefab.GetComponent<Kunai>();
            kunai.Damage = GetFinallDamage(m_skill1_damage_ratio, m_damage_level_ratio);
            kunai.ReflectCount = m_reflect_count;
        }
    }   
    

    protected override void ApplyLevelUpEffect(int level)
    {
        CheckSkillEvolve(m_skill_id);
        m_damage_level_ratio += m_damage_levelup_ratio;
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
