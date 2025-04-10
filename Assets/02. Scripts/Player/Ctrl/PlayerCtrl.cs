using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{ 
    private Rigidbody2D m_rigid;
    public SpriteRenderer m_sprite_renderer;
    private SkillManager m_skill_manager;

    [SerializeField]
    private PlayerStat m_origin_stat;
    public PlayerStat OriginStat { get { return m_origin_stat; } private set { m_origin_stat = value; } }
    [SerializeField]
    private PlayerStat m_stat;
    public PlayerStat Stat { get { return m_stat; } private set {m_stat = value; } }
    public JoyStickCtrl joyStick { get; private set; }
    public Animator Animator { get; private set; }

    private float m_regen_time = 0;
    private float m_regen_cool_time = 1f;

    private bool m_invincibility;
    private bool m_is_dead;

    private void Awake()
    {
        //GetCalculatedStat();
        InitStat();
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.Playing, GameManager.Instance.Playing);
        GameEventBus.Subscribe(GameEventType.Setting, GameManager.Instance.Setting);
        GameEventBus.Subscribe(GameEventType.Selecting, GameManager.Instance.Selecting);
        GameEventBus.Subscribe(GameEventType.Dead, GameManager.Instance.Dead);
        GameEventBus.Subscribe(GameEventType.Clear, GameManager.Instance.Clear);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.Playing, GameManager.Instance.Playing);
        GameEventBus.Unsubscribe(GameEventType.Setting, GameManager.Instance.Setting);
        GameEventBus.Unsubscribe(GameEventType.Selecting, GameManager.Instance.Selecting);
        GameEventBus.Unsubscribe(GameEventType.Dead, GameManager.Instance.Dead);
        GameEventBus.Unsubscribe(GameEventType.Clear, GameManager.Instance.Clear);           
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
        OriginStat.HP += GameManager.Instance.CalculatedStat.HP;
        OriginStat.AtkDamage += GameManager.Instance.CalculatedStat.ATK;
        OriginStat.HpRegen += GameManager.Instance.CalculatedStat.HP_REGEN;
    }

    public void InitStat()
    {
        Stat = ScriptableObject.CreateInstance<PlayerStat>();
        if(OriginStat == null)
        {
            Debug.Log("스탯이 없음");
        }
        Stat.HP = OriginStat.HP;
        Stat.HpRegen = OriginStat.HpRegen;
        Stat.MoveSpeed = OriginStat.MoveSpeed;
        Stat.AtkDamage = OriginStat.AtkDamage;
        Stat.BulletSize = OriginStat.BulletSize;
        Stat.ExpBonusRatio = OriginStat.ExpBonusRatio;
        Stat.CoolDownDecreaseRatio = OriginStat.CoolDownDecreaseRatio;
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

        if(Stat.HP <= 0f)
        {
            Dead();
        }
    }

    public void Dead()
    {
        GameEventBus.Publish(GameEventType.Dead);

        m_is_dead = true;
        Animator.SetTrigger("Death");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            if(m_is_dead)
            {
                return;
            }

            if(m_invincibility is true)
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