using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    private Skill m_skill;
    public Skill Skill
    {
        get { return m_skill; }
        private set { m_skill = value; }
    }

    [Header("스킬의 이미지")]
    [SerializeField] private Image m_skill_image;
    public Image Image
    {
        get { return m_skill_image; }
        private set { m_skill_image = value; }
    }

    private void SetAlpha(float alpha)
    {
        Color color = m_skill_image.color;
        color.a = alpha;
        m_skill_image.color = color;
    }

    public void Add(Skill skill)
    {
        Skill = skill;
        Image.sprite = Skill.Image;

        SetAlpha(1f);
    }

    public void Clear()
    {
        Skill = null;
        Image.sprite = null;

        SetAlpha(0f);
    }
}
