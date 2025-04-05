using UnityEngine;

[CreateAssetMenu(fileName = "New Active Skill", menuName = "Scriptable Object/Create Active Skill")]
public class ActiveSkill : Skill
{
    [Space(30)]
    [Header("진화 스킬")]
    [SerializeField] private Skill m_evolution_skill;
    public Skill Evolution
    {
        get { return m_evolution_skill; }
    }
}
