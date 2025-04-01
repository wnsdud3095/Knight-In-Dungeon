using UnityEngine;
using System.Collections.Generic;

public class PlayerCtrl : MonoBehaviour
{ 
    private Rigidbody2D m_rigid;
    private SpriteRenderer m_sprite_renderer;
    private SkillManager m_skill_manager;

    [SerializeField]
    private PlayerStat m_origin_stat;
    public PlayerStat OriginStat { get { return m_origin_stat; } private set { m_origin_stat = value; } }
    [SerializeField]
    private PlayerStat m_stat;
    public PlayerStat Stat { get; private set; }
    public JoyStickCtrl joyStick { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
        //GetCalculatedStat();
        InitStat();
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
        Move();

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
}
