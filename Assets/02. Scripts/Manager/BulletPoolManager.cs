using UnityEngine;
using System.Collections.Generic;

public class BulletPoolManager : MonoBehaviour
{
    public GameObject[] m_prefabs;

    public List<GameObject>[] m_pools;

    void Awake()
    {
        m_pools= new List<GameObject>[m_prefabs.Length];

        for (int i = 0; i < m_pools.Length; i++)
        {
            m_pools[i]= new List<GameObject>();
        }
    }

    void Update()
    {
        
    }

    public GameObject Get(SkillBullet bullet_name)
    {
        GameObject select = null;

        foreach(GameObject prefab in m_pools[(int)bullet_name]) // 선택된 아이디의 풀에 비활성화된 오브젝트에 접근
        {
            if(!prefab.activeSelf) 
            {
                select = prefab; //반환을 위해 select에 할당
                select.SetActive(true);
                break; // 하나만 필요
            }
        }

        if(!select)
        {
            select = Instantiate(m_prefabs[(int)bullet_name]);
            m_pools[(int)bullet_name].Add(select);
        }

        return select;
    }
}
