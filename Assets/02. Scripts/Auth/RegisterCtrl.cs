using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using System.Collections;
using Firebase;

public class RegisterCtrl : MonoBehaviour
{
    [Header("회원가입 패널")]
    [SerializeField] private Animator m_register_ui_object;

    [Header("이메일 필드")]
    [SerializeField] private TMP_InputField m_email_input_field;

    [Header("비밀번호 필드")]
    [SerializeField] private TMP_InputField m_password_input_field;

    [Header("중복확인 버튼")]
    [SerializeField] private Button m_check_button;

    [Header("중복확인 라벨")]
    [SerializeField] private TMP_Text m_check_label;

    [Header("회원가입 버튼")]
    [SerializeField] private Button m_register_button;

    FirebaseAuth m_auth;

    private Coroutine m_check_coroutine;
    private bool m_is_checked = false;

    private void Awake()
    {
        m_auth = FirebaseAuth.DefaultInstance;
    }

    public void Initialize()
    {
        m_email_input_field.text = "";
        m_password_input_field.text = "";
    }

    public async void Button_Check()
    {
        try
        {
            if(string.IsNullOrEmpty(m_email_input_field.text))
            {
                if(m_check_coroutine is not null)
                {
                    StopCoroutine(m_check_coroutine);
                }
                StartCoroutine(CheckEmailCoroutine("<color=red>이메일을 입력해주세요.</color>"));
                return;
            }

            Color color = m_check_label.color;
            color.a = 1f;
            m_check_label.color = color;
            m_check_label.text = "<color=#808080>이메일 중복 확인 중...</color>";

            try
            {
                await m_auth.CreateUserWithEmailAndPasswordAsync(m_email_input_field.text, "jxngminjxngmin");

                if (m_auth.CurrentUser is not null)
                {
                    await m_auth.CurrentUser.DeleteAsync();

                    if(m_check_coroutine is not null)
                    {
                        StopCoroutine(m_check_coroutine);
                    }
                    StartCoroutine(CheckEmailCoroutine("<color=green>사용 가능한 이메일입니다.</color>"));

                    m_is_checked = true;
                }
            }
            catch (FirebaseException ex)
            {
                switch(ex.ErrorCode)
                {
                    case 8:
                        if(m_check_coroutine is not null)
                        {
                            StopCoroutine(m_check_coroutine);
                        }
                        StartCoroutine(CheckEmailCoroutine("<color=red>이미 등록된 이메일입니다.</color>"));

                        m_is_checked = false;
                        break;
                    
                    default:
                        if(m_check_coroutine is not null)
                        {
                            StopCoroutine(m_check_coroutine);
                        }
                        StartCoroutine(CheckEmailCoroutine("<color=red>이메일 형식을 확인해주세요.</color>"));

                        m_is_checked = false;
                        break;
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogError($"오류 발생 : {ex.Message}");
            m_is_checked = false;
        }
    }

    public void Button_Register()
    {
        if(m_is_checked is false)
        {
            if(m_check_coroutine is not null)
            {
                StopCoroutine(m_check_coroutine);
            }
            StartCoroutine(CheckEmailCoroutine("<color=red>이메일 중복 확인을 해주세요.</color>"));
            return;
        }

        m_auth.CreateUserWithEmailAndPasswordAsync(m_email_input_field.text, m_password_input_field.text).ContinueWith
        (
            task => {
                if(!task.IsCanceled && !task.IsFaulted)
                {
                    FirebaseUser new_user = task.Result.User;

                    UserData new_user_data = new UserData(new_user.UserId);
                    DataManager.Instance.SaveUserData(new_user_data);
                }
            }
        );

        Button_Exit();
    }

    public void Button_Exit()
    {
        m_register_ui_object.SetBool("Open", false);
    }

    private IEnumerator CheckEmailCoroutine(string text)
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
