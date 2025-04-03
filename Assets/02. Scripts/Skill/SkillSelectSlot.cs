using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectSlot : MonoBehaviour
{
    [Header("스킬 스크립터블 오브젝트")]
    [SerializeField] private Skill m_skill;

    [Space(30)][Header("스킬 선택 슬롯 UI 컴포넌트")]
    [Header("스킬 이미지")]
    [SerializeField] private Image m_skill_image;

    [Header("스킬 강화 라벨")]
    [SerializeField] private TMP_Text m_skill_reinforcement_label;

    [Header("스킬 이름 라벨")]
    [SerializeField] private TMP_Text m_skill_name_label;

    [Header("스킬 설명 라벨")]
    [SerializeField] private TMP_Text m_skill_description_label;

    private PlayerSkillBase m_skill_base;

    private void Awake()
    {
        SetSkill();   
    }

    private void OnEnable()
    {
        m_skill_image.sprite = m_skill.Image;
        SetAlpha(1f);

        m_skill_reinforcement_label.text = $"+{m_skill_base.Level}";

        m_skill_name_label.text = m_skill.Name;
        
        if(m_skill.Type is SkillType.Active)
        {
            if(m_skill_base.Level == 1)
            {
                m_skill_description_label.text = m_skill.Description[0];
            }
            else if(m_skill_base.Level % 2 == 0)
            {
                m_skill_description_label.text = $"{m_skill.Description[1]}\n{m_skill.Description[3]}";
            }
            else
            {
                m_skill_description_label.text = $"{m_skill.Description[1]}\n{m_skill.Description[2]}";
            }
        }
        else
        {
            m_skill_description_label.text = m_skill.Description[0];
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = m_skill_image.color;
        color.a = alpha;
        m_skill_image.color = color;
    }

    private void SetSkill()
    {
        switch(m_skill.ID)
        {
            case 0:
                m_skill_base = GameObject.Find("Skill Manager").GetComponent<Skill1_KunaiThorw>();
                break;
            
            case 1:
                break;
            
            case 2:
                break;
            
            case 3:
                break;
            
            case 4:
                break;
            
            case 5:
                break;

            case 10:
                break;
            
            case 11:
                break;
            
            case 12:
                break;
            
            case 13:
                break;
            
            case 14:
                break;
            
            case 15:
                break;
        }
    }

    public void Button_Slot()
    {
        GameEventBus.Publish(GameEventType.Playing);
        
        m_skill_base.LevelUP();

        GameObject.Find("Select UI").GetComponent<Animator>().SetBool("Open", false);
    }
}
