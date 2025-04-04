using System.Collections;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Animator m_animator;
    private Coroutine m_coroutine;

    [SerializeField] private SpriteRenderer m_box_renderer;
    [SerializeField] private SpriteRenderer m_shadow_renderer;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();   
    }

    private void OnEnable()
    {
        SetColor(1f);
    }

    private void OnDisable()
    {
        m_coroutine = null;
        m_animator.ResetTrigger("Open");
    }

    private void SetColor(float alpha)
    {
        Color color = m_box_renderer.color;
        color.a = alpha;
        m_box_renderer.color = color;

        color = m_shadow_renderer.color;
        color.a = alpha;
        m_shadow_renderer.color = color;
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Skill"))
        {
            if(m_coroutine is not null)
            {
                return;
            }

            m_animator.SetTrigger("Open");

            m_coroutine = StartCoroutine(InstantiateItem());
        }
    }

    private IEnumerator InstantiateItem()
    {
        yield return new WaitForSeconds(0.5f);

        int item_code = UnityEngine.Random.Range(0, 3);

        GameObject item = null;
        switch(item_code)
        {
            case 0:
                item = ObjectManager.Instance.GetObject(ObjectType.Item_Potion);
                break;

            case 1:
                item = ObjectManager.Instance.GetObject(ObjectType.Item_Magnet);
                break;
        
            case 2:
                item = ObjectManager.Instance.GetObject(ObjectType.Item_Bomb);
                break;

            default:
                Debug.LogWarning($"[Box] Unknown item_code: {item_code}");
                break;
        }

        item.transform.position = transform.position;


        yield return new WaitForSeconds(0.5f);

        ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Item_Box);
    }
}
