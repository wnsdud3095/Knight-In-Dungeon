using System.Collections;
using Fusion;
using UnityEngine;

public class Box : NetworkBehaviour
{
    private Animator m_animator;
    private Coroutine m_coroutine;

    [SerializeField] private SpriteRenderer m_box_renderer;
    [SerializeField] private SpriteRenderer m_shadow_renderer;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();   
    }

    private void ResetTrigger()
    {
        if(HasStateAuthority)
        {
            RPC_ResetTrigger();
        }
        else
        {
            RPC_RequestResetTrigger();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestResetTrigger()
    {
        RPC_ResetTrigger();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ResetTrigger()
    {
        m_animator.ResetTrigger("Open");
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if(!HasStateAuthority)
        {
            return;
        }

        if(coll.CompareTag("Skill") && m_coroutine is null)
        {
            SetTrigger();
            InstantiateItem();
        }
    }

    private void SetTrigger()
    {
        if(HasStateAuthority)
        {
            RPC_SetTrigger();
        }
        else
        {
            RPC_RequestSetTrigger();
        }
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_RequestSetTrigger()
    {
        RPC_SetTrigger();
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SetTrigger()
    {
        m_animator.SetTrigger("Open");
    }

    public void InstantiateItem()
    {
        if(HasStateAuthority)
        {
            RPC_InstantiateItem();
        }
        else
        {
            RPC_ReqeustInstantiateItem();
        }
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_ReqeustInstantiateItem()
    {
        RPC_InstantiateItem();
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_InstantiateItem()
    {
        if(m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
        }

        m_coroutine = StartCoroutine(CoInstantiateItem());
    }

    private IEnumerator CoInstantiateItem()
    {
        if(!HasStateAuthority)
        {
            yield break;
        }

        yield return new WaitForSeconds(0.5f);

        int item_code = UnityEngine.Random.Range(0, 4);

        GameObject item = null;
        switch(item_code)
        {
            case 0:
                item = GameManager.Instance.NetworkObjectManager.GetPrefab(ObjectType.Item_Potion);
                break;

            case 1:
                item = GameManager.Instance.NetworkObjectManager.GetPrefab(ObjectType.Item_Magnet);
                break;
        
            case 2:
                item = GameManager.Instance.NetworkObjectManager.GetPrefab(ObjectType.Item_Bomb);
                break;

            case 3:
                item = GameManager.Instance.NetworkObjectManager.GetPrefab(ObjectType.Item_MoneyBag);
                break;
        }

        GameManager.Instance.NowRunner.Spawn(item, transform.position);

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.NowRunner.Despawn(GetComponent<NetworkObject>());
        ResetTrigger();
        m_coroutine = null;
    }
}
