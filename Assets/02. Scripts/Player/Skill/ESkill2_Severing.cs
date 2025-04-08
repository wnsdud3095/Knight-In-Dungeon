using UnityEngine;

public class ESkill2_Severing : Skill2_Severing
{
    private float m_e_skill2_cool_time = 1.5f;
    private float m_damage_e_level_ratio = 2f; // 스킬 만랩의 레벨별 공격력 배수
    private float m_e_heal = 0.5f;

    void Start()
    {
        m_cool_time = m_e_skill2_cool_time;

        Animator[] animators = GameManager.Instance.Player.transform.GetComponentsInChildren<Animator>(true);
        foreach (Animator animator in animators)
        {
            if (animator.gameObject.name == "ESeveringEffect")
            {
                m_effect = animator.gameObject;
            }
        }

        m_effect.GetComponent<ESevering>().Damage = GetFinallDamage(m_skill2_damage_ratio, m_damage_e_level_ratio);
        m_effect.GetComponent<ESevering>().Heal = m_e_heal;
    }

}
