using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerCtrl : NetworkBehaviour
{ 
    public SpriteRenderer SpriteRenderer { get; private set; }
    private SkillManager m_skill_manager;

    public NetworkTransform NetTransform { get; private set; }

    [Networked] public NetworkBool IsFlippedX { get;set; }
    [Networked] public NetworkBool IsMoving { get; set; }

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
    private bool m_is_dead;

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

    public override void Spawned()
    {
        if(HasInputAuthority == false ) //내가 조작 가능한 오브젝트인지 판단(내 input을 처리할 권한이 있는 오브젝트) 미사용시 한쪽의 인풋으로 이 스크립트 가진 오브젝트 둘다 움직임(Shared Mode라)
        {
            joyStick = null;
            return;
        }
        OriginStat = CloneStat(m_stat_scriptable);
        GetCalculatedStat();
        m_stat = CloneStat(OriginStat);
        if (m_stat == null) Debug.Log("스탯 널");
        m_skill_manager = GameObject.Find("Skill Manager").GetComponent<SkillManager>();
        joyStick = GameObject.Find("TouchPanel").GetComponent<JoyStickCtrl>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        NetTransform = GetComponent<NetworkTransform>();

        GameManager.Instance.Player = this;

        GameEventBus.Publish(GameEventType.Playing);
    }
    public override void FixedUpdateNetwork()
    {
        if (!HasInputAuthority) return;
        if (GameManager.Instance.GameState != GameEventType.Playing)
        {
            //m_character_ctrl.Move(Vector3.zero);
            return;
        }
        if (GetInput<NetworkInputData>(out var input))
        {
            Move(input.MoveDirection);
        }
        HpRegen();

        m_skill_manager.UseSkills();
    }

    public void GetCalculatedStat()
    {
        //OriginStat = CloneStat(m_stat_scriptable);
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

    public override void Render() //네트워크 예측 보간 처리 후에 호출되는 Update(), 모든 클라이언트에서 실행됨, 시각적인 요소만 처리하는데 적합
    {                               //FixedUpdateNetwork와 차이는 FixedUpdateNetwork는 네트워크 논리 실행용이라 클라이언트끼리 달라질수도 있음 Render는 모든 플레이어에게 동일한 결과를 보여주기위해 존재
                                    //움직임은 Render에서 안해도 똑같아 보이는 이유는 NetworkTransform같은 컴포넌트들이 보간/예측해서 알아서 부드럽게 처리해줌

        if (SpriteRenderer == null || Animator == null) return;

        SpriteRenderer.flipX = IsFlippedX;
        Animator.SetBool("IsMove", IsMoving);
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

    private void Move(Vector2 input_vector)
    {
        //m_character_ctrl.Move(input_vector * Stat.MoveSpeed * Runner.DeltaTime); 캐릭터까지 회전해버림
        //rigidbody.linearvelocity 는 버벅임이 심함 
        //m_rigid.linearVelocity = input_vector * Stat.MoveSpeed; //Runner.DeltaTime는 안곱함
        NetTransform.transform.Translate(input_vector * Stat.MoveSpeed * Runner.DeltaTime);
        transform.rotation = Quaternion.identity;
        if (input_vector.x > 0)
        {
            IsFlippedX = false;
            //SpriteRenderer.flipX = false;
        }
        else if (input_vector.x < 0)
        {
            IsFlippedX = true;
            //SpriteRenderer.flipX = true;
        }
        IsMoving = input_vector.sqrMagnitude > 0;
        //Animator.SetBool("IsMove", input_vector.sqrMagnitude > 0);
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
        if(GameManager.Instance.GameState is not GameEventType.Playing)
        {
            return;
        }

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
        else if(collision.CompareTag("Projectile"))
        {
            if(m_is_dead)
            {
                return;
            }

            if(m_invincibility is true)
            {
                return;
            }

            UpdateHP(-collision.GetComponent<Arrow>().ATK);

            StartCoroutine(Invincibility());
        }
    }

    private IEnumerator Invincibility()
    {
        Color color = SpriteRenderer.color;
        color.a = 100f / 255f;
        SpriteRenderer.color = color;

        float elasped_time = 0f;
        float target_time = 1f;

        while(elasped_time <= target_time)
        {
            if(GameManager.Instance.GameState is GameEventType.Playing)
            {
                elasped_time += Time.deltaTime;
            }

            yield return null;
        }

        color.a = 1f;
        SpriteRenderer.color = color;

        m_invincibility = false;
    }
}