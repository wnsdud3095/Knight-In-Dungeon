using UnityEngine;

public class Skill1_KunaiThorw : PlayerSkillBase
{
    [SerializeField]
    private float m_skill1_cool_time = 4f;
    private int m_kunal_count = 1;
    private float m_speed = 3f;
    private float m_damage;

    void Start()
    {
        m_cool_time = m_skill1_cool_time;
    }

    public override void UseSKill()
    {
        CoolTime();
        if(m_can_use)
        {
            for (int i = 0; i < m_kunal_count; i++)
            {
                var prefab = GameManager.Instance.BulletPool.Get(SkillName.KunaiThrow);
                prefab.transform.SetParent(GameManager.Instance.BulletPool.transform);
                prefab.transform.position = GameManager.Instance.Player.transform.position;

                prefab.transform.rotation = Quaternion.LookRotation(Vector3.forward, GameManager.Instance.Player.joyStick.GetInputVector()); //z축을 기준으로 벡터 방향을 바라보게 회전 시킴
                prefab.transform.Translate(Vector3.up * 2);
            }
        }
    }



    protected override void ApplyLevelUpEffect(int level)
    {
        throw new System.NotImplementedException();
    }
}
