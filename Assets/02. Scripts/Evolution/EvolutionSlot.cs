using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvolutionSlot : MonoBehaviour
{
    [Header("능력치 강화 타입")]
    [SerializeField] private EvolutionType m_evolution_type;
    public EvolutionType Type
    {
        get { return m_evolution_type; }
        set { m_evolution_type = value; }
    }

    [Header("능력치 계수")]
    [SerializeField] private float m_evolution_rate = 0f;
    public float Rate
    {
        get { return m_evolution_rate; }
        set  { m_evolution_rate = value; }
    }

    [Header("해금 레벨")]
    [SerializeField] private int m_unlock_level;
    public int Level
    {
        get { return m_unlock_level; }
        set { m_unlock_level = value; }
    }

    [Header("강화에 필요한 금액")]
    [SerializeField] int m_unlock_cost = 0;
    public int Cost
    {
        get { return m_unlock_cost; }
        set { m_unlock_cost = value; }
    }

    [Space(50)][Header("능력치 강화 슬롯의 UI 컴포넌트")]
    [Header("파이프 이미지")]
    [SerializeField] private Image m_pipe_image;

    [Header("능력치 강화 이름 라벨")]
    [SerializeField] private TMP_Text m_name_label;

    [Header("능력치 강화 설명 라벨")]
    [SerializeField] private TMP_Text m_description_label;

    [Header("능력치 강화 버튼")]
    [SerializeField] private Button m_evolution_button;

    [Header("능력치 강화 버튼의 라벨")]
    [SerializeField] private TMP_Text m_button_label;

    [Header("비활성화 이미지")]
    [SerializeField] private Image m_disable_image;

    [Header("해금 조건 레벨 라벨")]
    [SerializeField] private TMP_Text m_unlock_label;

    private void Awake()
    {
        UpdateSlotState();
    }

    private void Update()
    {
        UpdateSlotState();   
    }

    public void UpdateSlotState()
    {
        SetSlotLabel();
        SetPipeColor(24f/255f, 24f/255f, 24f/255f, 1f);

        if(DataManager.Instance.Data.m_user_level < m_unlock_level)
        {
            m_disable_image.gameObject.SetActive(true);
        }
        else
        {
            m_disable_image.gameObject.SetActive(false);
        }

        if(Level <= DataManager.Instance.Data.m_evolution_level)
        {
            PastClear();
        }

        m_evolution_button.interactable = Cost > DataManager.Instance.Data.m_user_money ? false : true;

    }

    private void SetPipeColor(float r, float g, float b, float a)
    {
        m_pipe_image.color = new Color(r, g, b, a);
    }

    private void SetSlotLabel()
    {
        m_unlock_label.text = $"LV.{m_unlock_level}";
        m_button_label.text = NumberFormatter.FormatNumber(Cost);

        switch(Type)
        {
            case EvolutionType.HP:
                m_name_label.text = "<color=green>체력 강화</color>";
                m_description_label.text = $"체력 <color=#47D8CD>+{NumberFormatter.FormatNumber(Rate)}</color>";
                break;
            
            case EvolutionType.ATK:
                m_name_label.text  = "<color=#FF7F00>공격력 강화</color>";
                m_description_label.text = $"공격력 <color=#47D8CD>+{NumberFormatter.FormatNumber(Rate)}</color>";
                break;
            
            case EvolutionType.HP_REGEN:
                m_name_label.text = "<color=red>체력재생력 강화</color>";
                m_description_label.text = $"체력재생력 <color=#47D8CD>+{Rate}%</color>";
                break;
        }
    }

    public void Button_Evolution()
    {
        SoundManager.Instance.PlayEffect("Button Click");
        
        DataManager.Instance.Data.m_user_money -= Cost;

        m_evolution_button.gameObject.SetActive(false);

        SetPipeColor(255f/255f, 100f/255f, 100f/255f, 1f);

        if(Level > DataManager.Instance.Data.m_evolution_level)
        {
            DataManager.Instance.Data.m_evolution_level = Level;
        }
    }

    public void PastClear()
    {
        m_evolution_button.gameObject.SetActive(false);

        SetPipeColor(255f/255f, 100f/255f, 100f/255f, 1f);

        if(Level > DataManager.Instance.Data.m_evolution_level)
        {
            DataManager.Instance.Data.m_evolution_level = Level;
        }        
    }
}
