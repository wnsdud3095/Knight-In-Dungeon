using UnityEngine;

public class BoxCtrl : MonoBehaviour
{
    [SerializeField] private float m_out_radius = 10f;
    [SerializeField] private float m_in_radius = 5f;
    [SerializeField] private float m_timer = 0f;

    private void Update()
    {
        m_timer += Time.deltaTime;

        if(m_timer >= 60f)
        {
            m_timer = 0f;
            CreateBox();
        }
    }

    private void CreateBox()
    {
        Vector2 center = GameManager.Instance.Player.transform.position;

        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(m_in_radius, m_out_radius);

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        Vector2 spawnPos = center + offset;

        GameObject box = ObjectManager.Instance.GetObject(ObjectType.Item_Box);
        box.transform.position = spawnPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GameManager.Instance.Player.transform.position, m_in_radius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GameManager.Instance.Player.transform.position, m_out_radius);
    }
}
