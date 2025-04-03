using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public Pool_Manager pool;
    public Transform player;
    public float spawnDistance = 8f;
    public float maxDistance = 15f; 
    public float repositionDistance = 10f;

    private Vector2 lastPlayerPosition;
    private Vector2 playerDirection;

    private float timer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        lastPlayerPosition = player.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        Vector2 currentPos = player.position;
        playerDirection = (currentPos - lastPlayerPosition).normalized;
        lastPlayerPosition = currentPos;

        if (timer > 1f)
        {
            Spawn();
            timer = 0;
        }

        CheckAndRepositionEnemies();
    }

    void Spawn()
    {
        GameObject enemy = pool.Get(0);

        Vector2 spawnPosition = (Vector2)player.position + (playerDirection * spawnDistance);

        if (playerDirection == Vector2.zero)
        {
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            spawnPosition = (Vector2)player.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnDistance;
        }

        enemy.transform.position = spawnPosition;
        enemy.SetActive(true);

        StartCoroutine(DisableAfterTime(enemy, 10f));
    }



    void CheckAndRepositionEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(player.position, enemy.transform.position);

            if (distance > maxDistance)
            {
                Vector2 repositionPosition = (Vector2)player.position + (playerDirection * repositionDistance);

                if (playerDirection == Vector2.zero)
                {
                    float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
                    repositionPosition = (Vector2)player.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * repositionDistance;
                }

                enemy.transform.position = repositionPosition;
            }
        }
    }

    IEnumerator DisableAfterTime(GameObject enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (enemy != null)
        {
            enemy.SetActive(false);
        }
    }
}

