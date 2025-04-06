using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Pool_Manager : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;
    public static Pool_Manager Instance;

    void Awake()
    {
        Instance = this;
        pools = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {      
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int i)
    {
        GameObject select = null;

        foreach (GameObject item in pools[i])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(prefabs[i], transform);
            pools[i].Add(select);
        }

        return select;
    }

}
