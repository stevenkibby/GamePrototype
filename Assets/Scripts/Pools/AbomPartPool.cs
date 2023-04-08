using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbomPartPool : MonoBehaviour
{
    public GameObject[] dustPool;
    public GameObject[] deathPool;

    [SerializeField] int dustPoolSize = 5;
    [SerializeField] int deathPoolSize = 1;

    [SerializeField] GameObject dustParticle;
    [SerializeField] GameObject deathParticle;

    Quaternion dustParticleRotation = Quaternion.Euler(-90f, 0f, 0f);
    Vector3 deathParticleAdjustment = new Vector3(0f, 0.75f, 0f);

    void Awake()
    {
        PopulateDustPartPool();
        PopulateDeathPartPool();
    }

    void PopulateDustPartPool()
    {
        dustPool = new GameObject[dustPoolSize];

        for (int i = 0; i < dustPool.Length; i++)
        {
            dustPool[i] = Instantiate(dustParticle, transform.position, dustParticleRotation, transform);
            dustPool[i].SetActive(false);
        }
    }

    void PopulateDeathPartPool()
    {
        deathPool = new GameObject[deathPoolSize];

        for (int i = 0; i < deathPool.Length; i++)
        {
            deathPool[i] = Instantiate(deathParticle, (transform.position + deathParticleAdjustment), Quaternion.identity, transform);
            deathPool[i].SetActive(false);
        }
    }

    public void EnableDustPartInPool()
    {
        for (int i = 0; i < dustPool.Length; i++)
        {
            if (!dustPool[i].activeInHierarchy)
            {
                dustPool[i].SetActive(true);
                return;
            }
        }

        Debug.Log($"Dust Particle ran out");
    }

    public GameObject EnableDeathPartInPool()
    {
        for (int i = 0; i < deathPool.Length; i++)
        {
            if (!deathPool[i].activeInHierarchy)
            {
                deathPool[i].SetActive(true);
                return deathPool[i];
            }
        }

        Debug.Log($"Death Particle ran out");
        return null;
    }
}
