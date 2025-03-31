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
        if (Gacha.Level > DataManager.Instance.Data.m_user_level)
        {
            m_one_button.interactable = false;
            m_set_button.interactable = false;
            m_disabled_image.gameObject.SetActive(true);
            return;
        }
        else
        {
            m_disabled_image.gameObject.SetActive(false);
        }

        if (Gacha.Cost > DataManager.Instance.Data.m_user_money)
        {
            m_one_button.interactable = false;
            m_set_button.interactable = false;
            return;
        }

        if (Gacha.Cost * 9 > DataManager.Instance.Data.m_user_money)
        {
            m_one_button.interactable = true;
            m_set_button.interactable = false;
        }
        else
        {
            m_one_button.interactable = true;
            m_set_button.interactable = true;
        }
    }

    public void Button_OneBuy()
    {
        DataManager.Instance.Data.m_user_money -= Gacha.Cost;

        m_prize_ctrl.OpenUI(Gacha, 1);

        // 아이템 구매 사운드를 출력한다.
    }

    public void Button_SetBuy()
    {
        DataManager.Instance.Data.m_user_money -= Gacha.Cost * 9;

        m_prize_ctrl.OpenUI(Gacha, 10);

        // 아이템 구매 사운드를 출력한다.
    }
}
