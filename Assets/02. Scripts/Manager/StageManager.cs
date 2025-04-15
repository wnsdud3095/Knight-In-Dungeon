using UnityEngine;

public class StageManager : MonoBehaviour
{
    private float[] m_exp_arr = new float[39] { 10f, 20f, 30f, 40f, 50f, 60f, 70f, 80f, 90f, 100f
                                            , 200f, 210f, 220f, 230f, 240f, 250f, 260f, 270f, 280f, 290f
                                            , 410f, 420f, 430f, 440f, 450f, 460f, 470f, 480f, 490f, 500f
                                            , 1000f, 1010f, 1020f, 1030f, 1040f, 1050f, 1060f, 1070f, 1080f};

    private float m_game_timer;
    public float GameTimer
    {
        get { return m_game_timer; }
        set { m_game_timer = value; }
    }

    private float m_origin_timer;
    public float OriginTimer
    {
        get { return m_origin_timer; }
        set { m_origin_timer = value; }
    }

    private int m_player_level;
    public int Level
    {
        get { return m_player_level; }
    }

    private float m_max_exp;
    public float MaxExp
    {
        get { return m_max_exp; }
    }

    private float m_current_exp;
    public float CurrentExp
    {
        get { return m_current_exp; }
        set { m_current_exp = value; }
    }

    private int m_kill_count;
    public int Kill
    {
        get { return m_kill_count; }
        set { m_kill_count = value; }
    }

    [Header("스킬 선택 매니저")]
    [SerializeField] private SkillSelector m_skill_selector;

    private void Awake()
    {
        m_game_timer = 24 * 60f;

        m_player_level = 1;
        
        m_max_exp = m_exp_arr[m_player_level - 1] * DataManager.Instance.Data.m_current_stage * DataManager.Instance.Data.m_current_stage;
        m_current_exp = 0f;
    }

    private void Update()
    {
        if(GameManager.Instance.GameState == GameEventType.Playing)
        {
            m_game_timer -= Time.deltaTime;
            m_game_timer = Mathf.Clamp(m_game_timer, 0f, 30 * 60f);
        }

        if(m_current_exp >= m_max_exp)
        {
            m_player_level++;
            m_current_exp -= m_max_exp;

            if(m_player_level <= 39)
            {
                m_max_exp = m_exp_arr[m_player_level - 1] * DataManager.Instance.Data.m_current_stage * DataManager.Instance.Data.m_current_stage;
            }
            else
            {
                m_max_exp = m_exp_arr[38] * DataManager.Instance.Data.m_current_stage * DataManager.Instance.Data.m_current_stage;
            }

            m_skill_selector.OpenUI();
        }
    }
}