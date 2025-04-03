using System.Collections.Generic;
using UnityEngine;

public class ExpPool : MonoBehaviour
{
    public static ExpPool Instance; 

    public GameObject expPrefab;
    public int poolSize = 10;
    
    private Queue<GameObject> expPool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject exp = Instantiate(expPrefab);
            exp.SetActive(false);
            expPool.Enqueue(exp);
        }
    }

    public GameObject GetExp()
    {
        if (expPool.Count > 0)
        {
            GameObject exp = expPool.Dequeue();
            exp.SetActive(true);
            return exp;
        }
        else
        {
            GameObject newExp = Instantiate(expPrefab);
            return newExp;
        }
    }

    public void ReturnExp(GameObject exp)
    {
        exp.SetActive(false);
        expPool.Enqueue(exp);
    }
}
