using UnityEngine;

public class Skill2_Severing : PlayerSkillBase
{
    //데미지 관련
    protected float m_skill2_damage_ratio = 2f; // 스킬의 공격력 계수
    private float m_damage_level_ratio = 1f; // 레벨별 공격력 배수
    private float m_damage_levelup_ratio = 0.2f; //레벨업시 공격력 배수가 증가하는 수치


    protected GameObject m_effect;
    private float m_skill2_cool_time = 3f;

    private float m_heal = 0.2f;

    private float m_area_expand_ratio = 1.5f;
    private float m_cool_down_decrease = 0.7f;

    void Start()
    {
        m_cool_time = m_skill2_cool_time;

        Animator[] animators = GameManager.Instance.Player.transform.GetComponentsInChildren<Animator>(true);
        foreach (Animator animator in animators)
        {           
            if(animator.gameObject.name == "SeveringEffect")
            {
                m_effect = animator.gameObject;
            }
        }

        m_effect.GetComponent<Severing>().Damage = GetFinallDamage(m_skill2_damage_ratio, m_damage_level_ratio);

        m_effect.GetComponent<Severing>().Heal = m_heal;
    }


    public override void UseSKill()
    {
        CoolTime(m_cool_time);

        if(m_can_use)
        {
            m_effect.transform.localScale = Vector3.one * GameManager.Instance.Player.Stat.BulletSize;

            m_effect.SetActive(true);
        }
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        m_damage_level_ratio += m_damage_levelup_ratio;
        if (level % 2 ==0)
        {
            m_effect.GetComponent<Severing>().ExpandArea(m_area_expand_ratio);
        }
        else
        {
            m_cool_time -= m_cool_down_decrease;
        }

        m_effect.GetComponent<Severing>().Damage = GetFinallDamage(m_skill2_damage_ratio, m_damage_level_ratio);

    }


}
