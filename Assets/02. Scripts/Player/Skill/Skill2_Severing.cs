using UnityEngine;

public class Skill2_Severing : PlayerSkillBase
{
    protected GameObject m_effect;
    private float m_skill2_cool_time = 3f;
    private float m_damage;

    private float m_heal = 0.2f;

    private float m_damage_up_ratio = 1.2f;
    private float m_area_expand_ratio = 1.5f;
    private float m_cool_down_decrease = 0.7f;

    void Start()
    {
        m_cool_time = m_skill2_cool_time;
        m_damage = GameManager.Instance.Player.Stat.AtkDamage * 2;

        Animator[] animators = GameManager.Instance.Player.transform.GetComponentsInChildren<Animator>(true);
        foreach (Animator animator in animators)
        {           
            if(animator.gameObject.name == "SeveringEffect")
            {
                m_effect = animator.gameObject;
            }
        }

        m_effect.GetComponent<Severing>().Damage = m_damage;
        m_effect.GetComponent<Severing>().Heal = m_heal;
    }


    public override void UseSKill()
    {
        CoolTime(m_cool_time);

        if(m_can_use)
        {
            m_effect.SetActive(true);
        }
    }

    protected override void ApplyLevelUpEffect(int level)
    {
        m_damage *= m_damage_up_ratio;
        if(level % 2 ==0)
        {
            m_effect.GetComponent<Severing>().ExpandArea(m_area_expand_ratio);
        }
        else
        {
            m_cool_time -= m_cool_down_decrease;
        }

        m_effect.GetComponent<Severing>().Damage = m_damage;
    }


}
