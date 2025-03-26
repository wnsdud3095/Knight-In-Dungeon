using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GachaSlot : MonoBehaviour
{
    private Gacha m_gacha;
    public Gacha Gacha
    {
        get { return m_gacha; }
        set { m_gacha = value; }
    }

    [Space(30)] [Header("가챠 UI 컴포넌트")]
    [Header("가챠의 이름")]
    [SerializeField] private TMP_Text m_name_label;
    
    [Header("가챠의 설명")]
    [SerializeField] private TMP_Text m_description_label;

    [Header("가챠의 이미지")]
    [SerializeField] private Image m_image;

    [Header("가챠의 단일 버튼")]
    [SerializeField] private Button m_one_button;

    [Header("가챠의 단일 버튼 라벨")]
    [SerializeField] private TMP_Text m_one_button_label;

    [Header("가챠의 세트 버튼")]
    [SerializeField] private Button m_set_button;
    
    [Header("가챠의 세트 버튼 라벨")]
    [SerializeField] private TMP_Text m_set_button_label;

    [Header("가챠의 비활성화 패널")]
    [SerializeField] private Image m_disabled_image;

    private PrizeCtrl m_prize_ctrl;

    private void Awake()
    {
        m_prize_ctrl = GameObject.Find("Shop Manager").GetComponent<PrizeCtrl>();
    }

    private void SetAlpha(float alpha)
    {
        Color color = m_image.color;
        color.a = alpha;
        m_image.color = color;
    }

    public void AddGacha(Gacha gacha)
    {
        Gacha = gacha;
        
        m_name_label.text = Gacha.Name;
        m_description_label.text = Gacha.Description;
        m_image.sprite = Gacha.Image;

        m_one_button_label.text = $"<color=green>1회 구매</color> {Gacha.Cost}";
        m_set_button_label.text = $"<color=green>10회 구매</color> {Gacha.Cost * 9}";

        SetAlpha(1f);
    }

    public void DestroySlot()
    {
        Destroy(gameObject);
    }

    public void Refresh()
    {
        //  레벨이 안된다면 비활성화

        {
            m_one_button.interactable = true;
            m_set_button.interactable = true;

            m_disabled_image.gameObject.SetActive(false);
            
            return;
        }


        // 1개를 살 돈이 없다면
        // 1개 구매 버튼과 세트 구매 버튼을 비활성화

        // 10개를 살 돈이 없다면
        // 세트 구매 버튼만 비활성화
    }

    public void Button_OneBuy()
    {
        // 돈을 차감한다.

        m_prize_ctrl.OpenUI(Gacha, 1);

        // 아이템 구매 사운드를 출력한다.
    }

    public void Button_SetBuy()
    {
        // 돈을 차감한다.

        m_prize_ctrl.OpenUI(Gacha, 10);

        // 아이템 구매 사운드를 출력한다.
    }
}
