using System.Collections;
using Fusion;
using UnityEngine;

public abstract class EnemyCtrl : NetworkBehaviour
{
    [field: SerializeField] public Rigidbody2D Rigidbody { get; protected set; }
    [field: SerializeField] public SpriteRenderer Renderer { get; protected set; }
    [field: SerializeField] public Animator Animator { get; protected set; }
    [field: SerializeField] public CircleCollider2D Collider { get; protected set; }
    public NetworkTransform NetworkTransform {get; protected set; }
    

    protected Enemy m_scriptable_object;
    public Enemy Script
    {
        get { return m_scriptable_object; }
        set { m_scriptable_object = value; }
    }

    protected float HP { get; set; }
    protected float SPD { get; set; }
    protected bool IsDead { get; set; }

    protected Coroutine m_knockback_coroutine;
    protected Coroutine m_freeze_coroutine;

    public override void Spawned()
    {
        NetworkTransform = GetComponent<NetworkTransform>();
    }

    public virtual void Initialize()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<CircleCollider2D>();

        Animator.speed = 1f;
    }

    protected virtual void MoveTowardsPlayer()
    {
        var target_player = default(PlayerCtrl);
        if(GameManager.Instance.Player1 && GameManager.Instance.Player2)
        {
            target_player = Vector2.Distance(GameManager.Instance.Player1.transform.position, transform.position) 
                                    > Vector2.Distance(GameManager.Instance.Player2.transform.position, transform.position) ? GameManager.Instance.Player2 : GameManager.Instance.Player1;
        }
        else
        {
            target_player = GameManager.Instance.Player1;
        }
        
        Vector2 direction = target_player.transform.position - transform.position;
        if(direction == Vector2.zero)
        {
            return;
        }

        direction.Normalize();
        Rigidbody.linearVelocity = direction * SPD;

        SetFlip(target_player);
    }

    private void SetFlip(PlayerCtrl player)
    {
        if(HasStateAuthority)
        {
            RPC_SetFlip(player);
        }
        else
        {
            RPC_RequestSetFlip(player);
        }
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_RequestSetFlip(PlayerCtrl player)
    {
        RPC_SetFlip(player);
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SetFlip(PlayerCtrl player)
    {
        Renderer.flipX = player.transform.position.x < transform.position.x;
    }

    public void UpdateHP(float amount)
    {
        if(HasStateAuthority)
        {
            RPC_UpdateHP(amount);
        }
        else
        {
            RPC_RequestUpdateHP(amount);
        }
    }

    private void RPC_RequestUpdateHP(float amount)
    {
        RPC_UpdateHP(amount);
    }

    private void RPC_UpdateHP(float amount)
    {
        if(IsDead is true)
        {
            return;
        }

        HP += amount;

        if(HP <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        if(!HasStateAuthority)
        {
            return;
        }
        
        if(IsDead is true)
        {
            return;
        }

        IsDead = true;

        Rigidbody.linearVelocity = Vector2.zero;
        Rigidbody.simulated = false;

        Renderer.sortingOrder = 0;

        Collider.enabled = false;

        SetDieTrigger();

        if (m_knockback_coroutine != null)
        {
            StopCoroutine(m_knockback_coroutine);
            m_knockback_coroutine = null;
        }
        if (m_freeze_coroutine != null)
        {
            StopCoroutine(m_freeze_coroutine);
            m_freeze_coroutine = null;
        }

        GameObject.Find("Stage Manager").GetComponent<StageManager>().Kill++;

        if(Script.Boss)
        {
            GameObject.Find("Stage Manager").GetComponent<SpawnManager>().BossCount--;
        }

        InstantiateExp();

        Invoke("ReturnEnemy", 2f);
    }

    private void SetDieTrigger()
    {
        if(HasStateAuthority)
        {
            RPC_SetDieTrigger();
        }
        else
        {
            RPC_RequestSetDieTrigger();
        }
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_RequestSetDieTrigger()
    {
        RPC_SetDieTrigger();
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SetDieTrigger()
    {
        Animator.SetTrigger("Die");
    }

    protected void InstantiateExp()
    {
        GameObject exp = ObjectManager.Instance.GetObject(ObjectType.Exp);
        exp.transform.position = transform.position;

        exp.GetComponent<Exp>().SetExpAmount(Script.EXP);
    }

    protected void ReturnEnemy()
    {
        GameManager.Instance.NowRunner.Despawn(GetComponent<NetworkObject>());
    }

    public void KnockBack(Vector2 current_position, float amount)
    {
        if(m_knockback_coroutine is not null)
        {
            StopCoroutine(m_knockback_coroutine);
        }

        m_knockback_coroutine = StartCoroutine(CoKnockBack(current_position, amount));
    }

    public IEnumerator CoKnockBack(Vector2 current_position, float amount)
    {
        Vector2 direction = -((Vector2)GameManager.Instance.Player.transform.position - current_position).normalized;

        float elasped_time = 0f;
        float target_time = 0.5f;

        Vector2 kps = direction * ((amount - Script.AntiKnockback) / target_time);

        if(kps.magnitude > 0f)
        {
            while(elasped_time <= target_time)
            {
                while (GameManager.Instance.GameState is not GameEventType.Playing)
                {
                    yield return null;
                }

                elasped_time += Time.deltaTime;
                yield return null;

                float t = elasped_time / target_time;

                Rigidbody.MovePosition(Rigidbody.position + kps * Time.deltaTime);
            }
        }

        m_knockback_coroutine = null;
    }

    public void SlowEnter(float amount)
    {
        SPD *= amount + Script.AntiSlow;
    }

    public void SlowExit()
    {
        SPD = Script.SPD;
    }

    public void Freeze(float duration)
    {
        if(m_freeze_coroutine is not null)
        {
            StopCoroutine(m_freeze_coroutine);
        }

        m_freeze_coroutine = StartCoroutine(CoFreeze(duration));
    }

    private IEnumerator CoFreeze(float duration)
    {
        SPD = 0f;

        if(GameManager.Instance.GameState is not GameEventType.Playing)
        {
            yield return null;
        }

        yield return new WaitForSeconds(duration - Script.AntiFreeze);

        SPD = Script.SPD;
    }
}