using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbomLife : MonoBehaviour
{
    //Stats variables
    [SerializeField] int maxHealth = 1;
    [SerializeField] int currentHealth;
    //[SerializeField] int hitValue = 0;
    //[SerializeField] int deathValue = 1;

    //Spawn location variables
    Vector3 baseLocation = new Vector3(150f, 0f, 0f);
    Vector3 phillipAdjustment = new Vector3(8.04f, 1.56f, -9.8f);
    Vector3 eugeneAdjustment = new Vector3(-14.02f, 2.05f, -6.12f);
    Vector3 mildredAdjustment = new Vector3(8.71f, 2.57f, 4.89f);
    Vector3 spawnAdjustment;

    //Other variables
    [SerializeField] GameObject arrow;
    Rigidbody rb;
    MeshRenderer[] meshRenderers;
    GameObject ground;
    //Scoreboard scoreboard;
    AbomPartPool particlePool;
    AbomMovement moveScript;

    Vector3 hitVfxAdjust = new Vector3(0f, 0.75f, 0f);

    void Awake()
    {
        moveScript = GetComponent<AbomMovement>();
        rb = GetComponent<Rigidbody>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        particlePool = FindObjectOfType<AbomPartPool>();
        //scoreboard = FindObjectOfType<Scoreboard>();
        ground = GameObject.FindGameObjectWithTag("Ground");
        DetermineSpawnAdjustment();
    }

    void OnEnable()
    {
        ResetAbom();
        MoveToSpawnLocation();
    }

    void DetermineSpawnAdjustment()
    {
        switch (gameObject.tag)
        {
            case "Phillip":
                spawnAdjustment = phillipAdjustment;
                break;
            case "Eugene":
                spawnAdjustment = eugeneAdjustment;
                break;
            case "Mildred":
                spawnAdjustment = mildredAdjustment;
                break;
            default:
                Debug.Log("Can't determine Spawn Adjustment");
                break;
        }
    }

    void ResetAbom()
    {
        currentHealth = maxHealth;
        DisableAbom(false);
    }

    void MoveToSpawnLocation()
    {
        transform.position = baseLocation + spawnAdjustment;
    }

    void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject == ground) //emit small smoke cloud
        {
            particlePool.EnableDustPartInPool();
        }

        if (other.gameObject.tag == "Projectile") //Can call 2-3 times if arrow hits multiple abom colliders at same time before becoming kinematic!
        {
            DepleteHealth();

            // if (currentHealth >= 1)
            // {
            //     scoreboard.ModifyScore(hitValue);
            // }

            //else: scoreboard.ModifyScore(deathValue); &
            if (currentHealth <= 0)
            {
                StartCoroutine(KillAbom());
            }
        }
    }

    void DepleteHealth()
    {
        currentHealth --;
    }

    IEnumerator KillAbom()
    {
        GameObject currentDeathParticle = particlePool.EnableDeathPartInPool();
        rb.velocity = Vector3.zero;
        DisableAbom(true);

        while (currentDeathParticle.activeSelf)
        {
            yield return null;
        }

        gameObject.SetActive(false);
    }

    void DisableAbom(bool isDisabled)
    {
        moveScript.enabled = !isDisabled;
        rb.isKinematic = isDisabled;
        rb.detectCollisions = !isDisabled;
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = !isDisabled;
        }
    }
}
