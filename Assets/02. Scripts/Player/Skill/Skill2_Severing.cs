using UnityEngine;

public class Skill2_Severing : PlayerSkillBase
{
    private GameObject m_effect;
    private float m_skill2_cool_time = 3f;
    private int m_area_expand_level = 0;
    private float m_damage;

    void Start()
    {
        m_cool_time = m_skill2_cool_time;
        m_damage = GameManager.Instance.Player.Stat.AtkDamage;

        Animator[] animators = GameManager.Instance.Player.transform.GetComponentsInChildren<Animator>(true);
        foreach (Animator animator in animators)
        {
            
            if(animator.gameObject.name == "SeveringEffect")
            {
                m_effect = animator.gameObject;
            }
        }

        m_effect.GetComponent<Severing>().SetDamage(m_damage);
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



        m_effect.GetComponent<Severing>().SetDamage(m_damage);
    }


}
