using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolTemplate : MonoBehaviour
{
    //Creates and returns a pool of game objects
    public GameObject[] PopulatePool(GameObject[] pool, int poolSize, GameObject prefab, Transform parent)
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(prefab, parent);
            pool[i].SetActive(false);
        }

        return pool;
    }

    //Enables first disabled game object in the pool
    public void EnableObjectInPool(GameObject[] pool)
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
}
