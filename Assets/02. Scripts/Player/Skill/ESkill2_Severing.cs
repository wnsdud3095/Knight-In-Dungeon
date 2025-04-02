using UnityEngine;

public class ESkill2_Severing : Skill2_Severing
{
    private float m_e_skill2_cool_time = 1.5f;
    private float m_e_damage;

    private float m_e_heal = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_cool_time = m_e_skill2_cool_time;
        m_e_damage = GameManager.Instance.Player.Stat.AtkDamage * 4f;

        Animator[] animators = GameManager.Instance.Player.transform.GetComponentsInChildren<Animator>(true);
        foreach (Animator animator in animators)
        {
            if (animator.gameObject.name == "ESeveringEffect")
            {
                m_effect = animator.gameObject;
            }
        }

        m_effect.GetComponent<ESevering>().Damage = m_e_damage;
        m_effect.GetComponent<ESevering>().Heal = m_e_heal;
    }

}
