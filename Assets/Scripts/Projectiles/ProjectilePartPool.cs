using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePartPool : MonoBehaviour
{
    [SerializeField] GameObject dustParticle;
    [SerializeField] GameObject bloodParticle;
    [SerializeField] GameObject rockParticle;

    public GameObject[] dustPool;
    public GameObject[] bloodPool;
    public GameObject[] rockPool;

    [SerializeField] int poolSize = 2;
    Quaternion projectilePartAdjust = Quaternion.Euler(-90f, 0f, 0f);

    void Awake()
    {
        dustPool = PopulatePartPool(dustParticle, dustPool);
        bloodPool = PopulatePartPool(bloodParticle, bloodPool);
        rockPool = PopulatePartPool(rockParticle, rockPool);
    }

    GameObject[] PopulatePartPool(GameObject particle, GameObject[] particlePool)
    {
        particlePool = new GameObject[poolSize];

        for (int i = 0; i < particlePool.Length; i++)
        {
            particlePool[i] = Instantiate(particle, transform.position, projectilePartAdjust, transform);
            particlePool[i].SetActive(false);
        }

        return particlePool;
    }

    public void EnablePartInPool(GameObject[] particlePool)
    {
        for (int i = 0; i < particlePool.Length; i++)
        {
            if (!particlePool[i].activeInHierarchy)
            {
                particlePool[i].SetActive(true);
                return;
            }
        }

        Debug.Log($"{particlePool} Particle ran out");
    }
}
