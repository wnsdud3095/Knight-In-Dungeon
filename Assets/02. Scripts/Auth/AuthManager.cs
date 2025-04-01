using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;
using System.Collections;
using System;
using Firebase;
using Unity.VisualScripting;
public class AuthManager : MonoBehaviour
{
    [Header("이메일 필드")]
    [SerializeField] private TMP_InputField m_email_input_field;

    [Header("비밀번호 필드")]
    [SerializeField] private TMP_InputField m_password_input_field;

    [Header("로그인 버튼")]
    [SerializeField] private Button m_login_button;

    [Header("회원가입 버튼")]
    [SerializeField] private Button m_register_button;

    [Header("체크 라벨")]
    [SerializeField] private TMP_Text m_check_label;

    [Space(30)]
    [Header("회원가입 패널")]
    [SerializeField] private Animator m_register_ui_object;
    private RegisterCtrl m_register_ctrl;

    FirebaseAuth m_auth;

    private Coroutine m_check_coroutine;

    private void Awake()
    {
        m_auth = FirebaseAuth.DefaultInstance;
        m_register_ctrl = m_register_ui_object.GetComponent<RegisterCtrl>();
    }

    private void Start()
    {
        GameEventBus.Publish(GameEventType.None);
    }

    public async void Button_Login()
    {
        SoundManager.Instance.PlayEffect("Button Click");

        try
        {
            var result = await m_auth.SignInWithEmailAndPasswordAsync(m_email_input_field.text, m_password_input_field.text);

            if(result is not null && result.User is not null)
            {
                FirebaseUser user = result.User;

                StartCoroutine(LoadUserDataCoroutine(user.UserId));

                if(m_check_coroutine is not null)
                {
                    StopCoroutine(m_check_coroutine);
                }
                StartCoroutine(CheckCoroutine("<color=green>로그인에 성공했습니다.</color>"));
            }
        }
        catch(FirebaseException ex)
        {
            switch(ex.ErrorCode)
            {
                case 38:
                    if(m_check_coroutine is not null)
                    {
                        StopCoroutine(m_check_coroutine);
                    }
                    StartCoroutine(CheckCoroutine("<color=red>비밀번호를 입력하세요.</color>"));
                    break;
                
                default:
                    if(m_check_coroutine is not null)
                    {
                        StopCoroutine(m_check_coroutine);
                    }
                    StartCoroutine(CheckCoroutine("<color=red>일치하는 정보가 없습니다.</color>"));                
                    break;
            }
        }
    }

    public void Button_Register()
    {
        SoundManager.Instance.PlayEffect("Button Click");
        
        m_register_ui_object.SetBool("Open", true);
        m_register_ctrl.Initialize();
    }

    private IEnumerator LoadUserDataCoroutine(string user_id)
    {
        yield return null;

        DataManager.Instance.LoadUserData(user_id);
        LoadingManager.Instance.LoadScene("Title");
    }

    private IEnumerator CheckCoroutine(string text)
    {
        m_check_label.text = text;

        Color color = m_check_label.color;
        color.a = 1f;
        m_check_label.color = color;
        
        float elapsed_time = 0f;
        float target_time = 1f;

        while(elapsed_time < target_time)
        {
            elapsed_time += Time.deltaTime;

            float t = elapsed_time / target_time;
            color.a = Mathf.Lerp(1f, 0f, t);
            m_check_label.color = color;
            
            yield return null;   
        }

        color.a = 0f;
        m_check_label.color = color;
    }
}
