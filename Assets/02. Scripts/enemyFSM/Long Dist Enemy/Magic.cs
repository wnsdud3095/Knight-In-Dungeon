using UnityEngine;

public class Magic : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 3f; 
    private bool isClone = false; 
    private Transform player; 

    void Awake()
    {
        if (transform.parent == null) 
        {
            isClone = true;
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform; 
    }

    void Start()
    {
        if (isClone)
        {
            FacePlayer(); 
            Destroy(gameObject, lifeTime);
        }
    }

    void FacePlayer()
    {
        if (player == null) return; 
        Vector2 direction = (player.position - transform.position).normalized; 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        transform.rotation = Quaternion.Euler(0, 0, angle); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isClone && other.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }
}
