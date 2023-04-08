using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPoolTemplate : MonoBehaviour
{
    GameObject[] pool1;
    GameObject[] pool2;
    GameObject[] pool3;

    [SerializeField] int poolSize1;
    [SerializeField] int poolSize2;
    [SerializeField] int poolSize3;

    [SerializeField] GameObject particle1;
    [SerializeField] GameObject particle2;
    [SerializeField] GameObject particle3;

    void Awake()
    {
        PopulatePool(pool1, poolSize1, particle1, transform);
        PopulatePool(pool2, poolSize2, particle2, transform);
        PopulatePool(pool3, poolSize3, particle3, transform);
    }


    //Creates and returns a pool of particles
    GameObject[] PopulatePool(GameObject[] pool, int poolSize, GameObject particle, Transform parent)
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(particle, parent);
            pool[i].SetActive(false);
        }

        return pool;
    }

    //Plays first stopped particle in the pool. Call from other scripts unless particle plays on spawn
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
