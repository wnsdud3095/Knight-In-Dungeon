using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;
using System.Collections;
using System.Threading.Tasks;
using System;
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

    [Space(30)]
    [Header("회원가입 패널")]
    [SerializeField] private Animator m_register_ui_object;
    private RegisterCtrl m_register_ctrl;

    FirebaseAuth m_auth;

    private void Awake()
    {
        m_auth = FirebaseAuth.DefaultInstance;
        m_register_ctrl = m_register_ui_object.GetComponent<RegisterCtrl>();
    }

    public async void Button_Login()
    {
        try
        {
            var result = await m_auth.SignInWithEmailAndPasswordAsync(m_email_input_field.text, m_password_input_field.text);

            if(result is not null && result.User is not null)
            {
                FirebaseUser user = result.User;

                StartCoroutine(LoadUserDataCoroutine(user.UserId));
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void Button_Register()
    {
        m_register_ui_object.SetBool("Open", true);
        m_register_ctrl.Initialize();
    }

    private IEnumerator LoadUserDataCoroutine(string user_id)
    {
        yield return null;

        DataManager.Instance.LoadUserData(user_id);
    }
}
