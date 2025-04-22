using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Severing : BulletBase
{
    public float Damage { get; set; }

    public float Heal { get; set; }

    [SerializeField]
    private BoxCollider2D[] m_collders;
    protected int m_col_index = 0;

    public void ExpandArea(float ratio)
    {
        transform.localScale *= ratio;
    }

    public void Update()
    {
        GameStateCheck();
    }

    public virtual void ColActive()
    {
        if(m_col_index -1 <0)
        {
            m_collders[m_col_index].enabled = true;
            m_col_index++;
            return;
        }
        m_collders[m_col_index-1].enabled = false;
        m_collders[m_col_index].enabled = true;
        m_col_index++;
    }
    public virtual void AniEnd()
    {
        m_collders[m_col_index-1].enabled = false;
        m_col_index = 0;
        transform.gameObject.SetActive(false);
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            float heal_ratio = GameManager.Instance.Player.OriginStat.HP * (Heal / 100f);
            GameManager.Instance.Player.UpdateHP(heal_ratio);
            col.GetComponent<EnemyCtrl>().UpdateHP(-Damage);

            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);
            
            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = col.transform.position;
        }
    }
}
