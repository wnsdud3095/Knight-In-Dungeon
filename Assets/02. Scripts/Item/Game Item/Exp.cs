using Unity.VisualScripting;
using UnityEngine;

public class Exp : MonoBehaviour
{
    [Header("스프라이트 렌더러")]
    [SerializeField] private SpriteRenderer m_sprite_renderer;

    [Header("경험치 계수마다 교체할 이미지들의 목록")]
    [SerializeField] private Sprite[] m_exp_sprites;

    private int m_exp_amount;
    private bool m_is_magneted = false;
    public bool Magneted
    {
        get { return m_is_magneted; }
        set { m_is_magneted = value; }
    }

    private void OnDisable()
    {
        m_is_magneted = false;
    }

    private void Update()
    {
        if(m_is_magneted)
        {
            transform.position = Vector2.MoveTowards(transform.position, GameManager.Instance.Player.transform.position, 10f * Time.deltaTime);
        }   
    }

    public void SetExpAmount(int amount)
    {
        m_exp_amount = amount;

        if(m_exp_amount <= 5)
        {
            m_sprite_renderer.sprite = m_exp_sprites[0];
        }
        else if(m_exp_amount <= 10)
        {
            m_sprite_renderer.sprite = m_exp_sprites[1];
        }
        else
        {
            m_sprite_renderer.sprite = m_exp_sprites[2];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.Find("Stage Manager").GetComponent<StageManager>().CurrentExp += m_exp_amount * GameManager.Instance.Player.Stat.ExpBonusRatio;

            ObjectManager.Instance.ReturnObject(gameObject, ObjectType.Exp);
        }
    }
}
