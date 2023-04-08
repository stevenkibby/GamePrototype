using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MildredLife : MonoBehaviour
{
    //Upon collision with particle
    //Play death particle on own location
    //Add points (public)
    //Destroy self

    [SerializeField] GameObject arrow;
    [SerializeField] GameObject abominationsEmpty;
    [SerializeField] GameObject spawnAtRuntime;
    [SerializeField] ParticleSystem hitVFX;
    [SerializeField] ParticleSystem deathVFX;
    [SerializeField] ParticleSystem dustVFX;

    Scoreboard scoreboard;
    Vector3 spawnLocation;
    Vector3 hitVfxAdjust = new Vector3(0f, 0.75f, 0f);

    int health = 3;
    int hitValue = 0;
    int deathValue = 3;

    void Start()
    {
        scoreboard = FindObjectOfType<Scoreboard>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other = arrow)
        {
            DepleteHealth();

            if (health >= 1)
            {
                scoreboard.ModifyScore(hitValue);
                ProcessHitParticles();
            }

            else
            {
                scoreboard.ModifyScore(deathValue);
                ProcessDeathParticles();
                DetermineSpawnLocation();
                Invoke(nameof(SpawnNewAbom), 3f);
                DisableAbom();
                Destroy(gameObject, 3f);
                Invoke(nameof(EnableAbom), 2.9999f);
            }
        }
    }

    private void DisableAbom()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }

        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
    }

    private void EnableAbom()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = true;
        }

        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = true;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        //emit small smoke cloud
        ParticleSystem vfx = Instantiate(dustVFX, transform.position, Quaternion.Euler(-90f, 0f, 0f));
        vfx.transform.parent = spawnAtRuntime.transform;
    }

    private void DepleteHealth()
    {
        health --;
    }

    private void ProcessHitParticles()
    {
        ParticleSystem vfx = Instantiate(hitVFX, transform.position + hitVfxAdjust, Quaternion.identity);
        vfx.transform.parent = spawnAtRuntime.transform;
    }

    private void ProcessDeathParticles()
    {
        ParticleSystem vfx = Instantiate(deathVFX, transform.position + hitVfxAdjust, Quaternion.identity);
        vfx.transform.parent = spawnAtRuntime.transform;
    }

    private void DetermineSpawnLocation()
    {
        Vector3 baseLocation = new Vector3(150f, 0f, 0f);
        Vector3 mildredAdjustment = new Vector3(8.71f, 2.57f, 4.89f);
        spawnLocation = baseLocation + mildredAdjustment;
    }

    private void SpawnNewAbom()
    {
        GameObject newAbom = Instantiate(gameObject, spawnLocation, Quaternion.identity);
        newAbom.transform.parent = abominationsEmpty.transform;
        newAbom.name = "Mildred (Spawned)";
    }
}
