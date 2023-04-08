using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbomObjPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int spawnTime = 1;
    [SerializeField] int poolSize = 2;
    GameObject[] pool;
    bool waitingOnCoroutine = false;
    
    void Awake()
    {   
        PopulatePool();
        EnableObjectInPool();
    }

    void Update()
    {
        if (waitingOnCoroutine == false && isActive() == false)
        {
            waitingOnCoroutine = true;
            StartCoroutine(SpawnObject());
        }
    }

    void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(prefab, transform);
            pool[i].SetActive(false);
        }
    }

    void EnableObjectInPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return;
            }
        }
    }

    bool isActive()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i].activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator SpawnObject()
    {
        yield return new WaitForSeconds(spawnTime);
        EnableObjectInPool();
        waitingOnCoroutine = false;
    }
}
