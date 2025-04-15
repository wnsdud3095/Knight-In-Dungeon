using UnityEngine;

public class ShurikenRotater : MonoBehaviour
{
    [SerializeField]
    private float m_life_time = 0;
    public float LifeTime {get {return m_life_time;} set { m_life_time = value; } }

    public float SpinningSpeed { get; set; }

    void Update()
    {
        if (GameManager.Instance.GameState != GameEventType.Playing) return;

        if (m_life_time > 0)
        {
            transform.Rotate(Vector3.back * SpinningSpeed * Time.deltaTime);
            m_life_time -= Time.deltaTime;
        }
        else
        {
            ChildrenDisable();
            gameObject.SetActive(false);
        }
    }

    
    private void ChildrenDisable() // 하위 오브젝트들을 한번에 관리
    {
        Animator[] animators = transform.GetComponentsInChildren<Animator>();
        
        foreach (Animator animator in animators)
        {
            animator.gameObject.SetActive(false);
        }
    }
    
}
