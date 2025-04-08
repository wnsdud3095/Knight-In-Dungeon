using UnityEngine;

public class Meteor : MagicMissile
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_damage_up = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState != GameEventType.Playing) return;

        transform.Translate(Vector3.up * Speed * Time.deltaTime);

        LifeTimeCheck();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {        
        if (col.CompareTag("Enemy"))
        {
            //col.GetComponent<EnemyFSM>().TakeDamage(Damage);
            Debug.Log($"메테오 : {Damage}");
            EnemyController e_ctrl = col.GetComponent<EnemyController>();

            e_ctrl.StartCoroutine(e_ctrl.KnockBackRoutine(transform.position, 15f));

            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);

            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = col.transform.position;
        }
    }
}
