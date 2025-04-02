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

    private void OnEnable()
    {

    }
}
