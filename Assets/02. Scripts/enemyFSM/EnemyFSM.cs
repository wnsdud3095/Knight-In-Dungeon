using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState { Move, Die }

    [SerializeField] private EnemyData enemyData;
    private float currentHealth;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private Collider2D coll;
    public GameObject expOrbPrefab; 

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        InitEnemy();
    }

    void OnEnable()
    {

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        InitEnemy();
    }

    void InitEnemy()
    {
        isDead = false;
        currentHealth = enemyData.maxHealth;
        rb.simulated = true;
        coll.enabled = true;
        spriteRenderer.sortingOrder = 2;
        animator.SetTrigger("Move");
    }

    void FixedUpdate()
    {
        if (isDead) return;
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * enemyData.moveSpeed;

        animator.SetFloat("Speed", rb.linearVelocity.magnitude);
        spriteRenderer.flipX = player.position.x < transform.position.x;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        coll.enabled = false;
        spriteRenderer.sortingOrder = 1;
        animator.SetTrigger("Die");

        DropExp();

        Invoke("DisableEnemy", 2f);
    }

    void DisableEnemy()
    {
        gameObject.SetActive(false);
    }
    
    void DropExp()
    {
        GameObject exp_orb = ObjectManager.Instance.GetObject(ObjectType.Exp);
        exp_orb.GetComponent<Exp>().SetExpAmount(enemyData.Exp);
        
        exp_orb.transform.position = transform.position;
    }
}

