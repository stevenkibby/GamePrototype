using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileObjPool : MonoBehaviour
{
    [SerializeField] int poolSize = 10;
    public int PoolSize { get { return poolSize; } }

    [SerializeField] GameObject prefab; 
    [SerializeField] Transform parent; 
    GameObject[] pool;

    public GameObject[] CreateProjectileObjPool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(prefab, parent);
            pool[i].SetActive(false);
        }

        return pool;
    }
}