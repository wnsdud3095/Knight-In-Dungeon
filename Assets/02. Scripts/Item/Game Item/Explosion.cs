using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("캔버스 오브젝트")]
    [SerializeField] private Canvas m_canvas;

    private void Start()
    {
        Invoke("DestroyObject", 1f);
    }

    private void DestroyObject()
    {
        Destroy(m_canvas.gameObject);
    }
}
