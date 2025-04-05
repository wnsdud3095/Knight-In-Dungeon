using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Scriptable Object/Create Skill")]
public class Skill : ScriptableObject
{
    [Header("스킬의 고유한 ID")]
    [SerializeField] private int m_skil_id;
    public int ID
    {
        get { return m_skil_id; }
    }

    [Header("스킬의 타입")]
    [SerializeField] SkillType m_skill_type;
    public SkillType Type
    {
        get { return m_skill_type; }
    }

    [Header("스킬의 이름")]
    [SerializeField] private string m_skill_name;
    public string Name
    {
        get { return m_skill_name; }
    }

    [Header("스킬의 설명")]
    [SerializeField] private string[] m_skill_description = new string[3];
    public string[] Description
    {
        get { return m_skill_description; }
    }

    [Header("스킬의 이미지")]
    [SerializeField] private Sprite m_skill_image;
    public Sprite Image
    {
        get { return m_skill_image; }
    }

    [Header("조합 스킬")]
    [SerializeField] private Skill m_combination_skill;
    public Skill Combination
    {
        get { return m_combination_skill; }
    }
}
