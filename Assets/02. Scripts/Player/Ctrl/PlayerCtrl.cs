using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerCtrl : MonoBehaviour
{ 
    private Rigidbody2D m_rigid;
    public SpriteRenderer m_sprite_renderer;
    private SkillManager m_skill_manager;

    [SerializeField]
    private PlayerStat m_stat_scriptable; //캐릭터 기본 스탯 스크립터블 오브젝트
    public PlayerStat OriginStat { get; set; } // 기본스탯 + 진화스탯 , 런타임 스탯이 디버프 될시 값 복구를 위한 원본 값
    [SerializeField]
    private PlayerStat m_stat; // 런타임에서 현재 스탯 확인용
    public PlayerStat Stat { get { return m_stat; } private set {m_stat = value; } } // 런타임에 변경되는 스테이지 내부 스탯
    public JoyStickCtrl joyStick { get; private set; }
    public Animator Animator { get; private set; }

    private float m_regen_time = 0;
    private float m_regen_cool_time = 1f;

    private bool m_invincibility;
    private void Awake()
    {
        GetCalculatedStat();
        m_stat = CloneStat(OriginStat);
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.Playing, GameManager.Instance.Playing);
        GameEventBus.Subscribe(GameEventType.Setting, GameManager.Instance.Setting);
        GameEventBus.Subscribe(GameEventType.Selecting, GameManager.Instance.Selecting);        
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.Playing, GameManager.Instance.Playing);
        GameEventBus.Unsubscribe(GameEventType.Setting, GameManager.Instance.Setting);
        GameEventBus.Unsubscribe(GameEventType.Selecting, GameManager.Instance.Selecting);                
    }

    void Start()
    {
        m_skill_manager = GameObject.Find("Skill Manager").GetComponent<SkillManager>();
        joyStick = GameObject.Find("TouchPanel").GetComponent<JoyStickCtrl>();
        m_rigid = GetComponent<Rigidbody2D>();
        m_sprite_renderer= GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();

        GameEventBus.Publish(GameEventType.Playing);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.GameState != GameEventType.Playing)
        {
            m_rigid.linearVelocity = Vector2.zero;
            return;
        }

        Move();
        HpRegen();

        m_skill_manager.UseSkills();
    }

    public void GetCalculatedStat()
    {
        OriginStat = CloneStat(m_stat_scriptable);
        OriginStat.HP += GameManager.Instance.CalculatedStat.HP;
        OriginStat.AtkDamage += GameManager.Instance.CalculatedStat.ATK;
        OriginStat.HpRegen += GameManager.Instance.CalculatedStat.HP_REGEN;
    }

    public PlayerStat CloneStat(PlayerStat origin_stat)
    {
        PlayerStat new_stat = ScriptableObject.CreateInstance<PlayerStat>();
        if(OriginStat == null)
        {
            Debug.Log("스탯이 없음");
        }
        new_stat.HP = origin_stat.HP;
        new_stat.HpRegen = origin_stat.HpRegen;
        new_stat.MoveSpeed = origin_stat.MoveSpeed;
        new_stat.AtkDamage = origin_stat.AtkDamage;
        new_stat.BulletSize = origin_stat.BulletSize;
        new_stat.ExpBonusRatio = origin_stat.ExpBonusRatio;
        new_stat.CoolDownDecreaseRatio = origin_stat.CoolDownDecreaseRatio;
        return new_stat;
    }

    private void HpRegen()
    {
        if(m_regen_time <= m_regen_cool_time )
        {
            m_regen_time += Time.deltaTime;
        }
        else
        {
            m_regen_time = 0;
            UpdateHP(OriginStat.HP * (Stat.HpRegen / 100f)); //최대 체력의 HpRegen% 만큼 회복
        }
    }

    private void Move()
    {
        Vector2 input_vector = joyStick.GetInputVector();

        m_rigid.linearVelocity = new Vector2(input_vector.x * Stat.MoveSpeed, input_vector.y * Stat.MoveSpeed);
        if (input_vector.x > 0)
        {
            m_sprite_renderer.flipX = false;
        }
        else if (input_vector.x < 0)
        {
            m_sprite_renderer.flipX = true;
        }
        Animator.SetBool("IsMove", input_vector.sqrMagnitude > 0);
    }

    public void UpdateHP(float hp)
    {
        Stat.HP += hp;
        Stat.HP = Mathf.Clamp(Stat.HP, 0f, OriginStat.HP);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"[트리거 감지] 이름: {collision.name}, 위치: {collision.transform.position}, active: {collision.gameObject.activeSelf}, tag: {collision.tag}");
            if (m_invincibility is true)
            {
                return;
            }

            m_invincibility = true;

            UpdateHP(-collision.GetComponent<EnemyCtrl>().Script.ATK);

            StartCoroutine(Invincibility());
        }
    }

    private IEnumerator Invincibility()
    {
        Color color = m_sprite_renderer.color;
        color.a = 100f / 255f;
        m_sprite_renderer.color = color;

        yield return new WaitForSeconds(1f);

        color.a = 1f;
        m_sprite_renderer.color = color;

        m_invincibility = false;
    }
}
