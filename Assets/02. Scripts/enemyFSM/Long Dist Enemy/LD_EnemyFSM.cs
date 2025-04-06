using UnityEngine;
using System.Collections;

public class LD_EnemyFSM : MonoBehaviour
{
    public enum EnemyState { Move, Die, Attack }

    [SerializeField] private EnemyData enemyData;
    private float currentHealth;
    public float currentMoveSpeed;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private Collider2D coll;

    [SerializeField] private GameObject arrowPrefab;

    private bool canAttack = true;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private Transform firePoint;
    private LD_EnemyController LD_enemyControll;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        LD_enemyControll = GetComponent<LD_EnemyController>();
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
        currentMoveSpeed = enemyData.moveSpeed;
        rb.simulated = true;
        coll.enabled = true;
        spriteRenderer.sortingOrder = 2;
        animator.SetTrigger("Move");
    }

    void FixedUpdate()
    {
        if (isDead) return;

        float distance = Vector2.Distance(player.position, rb.position);

        MoveTowardsPlayer();

        if (distance <= enemyData.attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
        }
        else
        {
            animator.SetTrigger("Move");
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;


        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * currentMoveSpeed;

        animator.SetFloat("Speed", rb.linearVelocity.magnitude);
        spriteRenderer.flipX = player.position.x < transform.position.x;
    }

    public void Slow(float slowValue)
    {
        currentMoveSpeed = enemyData.moveSpeed * slowValue;
    }

    public void ClearSlow()
    {
        currentMoveSpeed = enemyData.moveSpeed;
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



    IEnumerator Attack()
    {
        while(LD_enemyControll.isFreezing)
        {
            yield return null;
        }

        canAttack = false;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();

            if (arrowRb != null)
            {
                Vector2 direction = (player.position - firePoint.position).normalized;
                arrowRb.linearVelocity = direction * 8f;
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
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
        GameObject expOrb = ExpPool.Instance.GetExp();
        expOrb.transform.position = transform.position;

        exp orbScript = expOrb.GetComponent<exp>();
        if (orbScript != null)
        {
            orbScript.SetExpAmount(enemyData.Exp);
        }
    }

    public EnemyData GetEnemyData()
    {
        return enemyData;
    }
}

