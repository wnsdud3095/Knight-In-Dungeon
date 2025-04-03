using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager m_instance;
    public static LoadingManager Instance
    {
        get
        {
            if(m_instance is null)
            {
                m_instance = FindFirstObjectByType<LoadingManager>();

                if(m_instance is null)
                {
                    m_instance = Create();
                }
            }

            return m_instance;
        }
    }

    private static LoadingManager Create()
    {
        return Instantiate(Resources.Load<LoadingManager>("Loading UI"));
    }

    [Header("로딩 UI의 캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("로딩 진행도를 출력할 TMP 라벨")]
    [SerializeField] private TMP_Text m_rate_label;

    private string m_target_scene_name;
    public string Current
    {
        get { return m_target_scene_name; }
    }

    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }   
        
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string scene_name)
    {
        GameEventBus.Publish(GameEventType.Loading);
        
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;

        m_target_scene_name = scene_name;

        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        m_rate_label.text = "0%";

        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(m_target_scene_name);
        op.allowSceneActivation = false;

        float elapsed_time = 0f;
        while(op.isDone is false)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                m_rate_label.text = (op.progress * 100).ToString("F0") + "%";
            }
            else
            {
                elapsed_time += Time.unscaledDeltaTime;
                m_rate_label.text = (Mathf.Lerp(0.9f, 1f, elapsed_time) * 100).ToString("F0") + "%";

                if(m_rate_label.text == "100%")
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private IEnumerator Fade(bool is_fade_in)
    {
        float elapsed_time = 0f;
        float target_time = 1f;

        while(elapsed_time < target_time)
        {
            elapsed_time += Time.unscaledDeltaTime;

            yield return null;

            m_canvas_group.alpha = is_fade_in ? Mathf.Lerp(0f, 1f, elapsed_time) : Mathf.Lerp(1f, 0f, elapsed_time);
        }

        if(is_fade_in is false)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == m_target_scene_name)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }   
    }
}