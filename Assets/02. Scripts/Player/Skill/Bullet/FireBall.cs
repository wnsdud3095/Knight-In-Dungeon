using UnityEngine;

public class FireBall : MagicMissile
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        m_damage_up = 1.6f;
    }

    // Update is called once per frame
    new void Update()
    {
        GameStateCheck();
        if (GameManager.Instance.GameState != GameEventType.Playing) return;

        transform.Translate(Vector3.up * Speed * Time.deltaTime);

        LifeTimeCheck();
    }

    private new void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyCtrl>().UpdateHP(-Damage);
            GameObject damage_indicator = ObjectManager.Instance.GetObject(ObjectType.DamageIndicator);

            damage_indicator.GetComponent<DamageIndicator>().Initialize(Damage);
            damage_indicator.transform.position = col.transform.position;
        }
    }
}
