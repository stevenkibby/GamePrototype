using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EugeneLife : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject abominationsEmpty;
    [SerializeField] GameObject spawnAtRuntime;
    [SerializeField] ParticleSystem hitVFX;
    [SerializeField] ParticleSystem deathVFX;
    [SerializeField] ParticleSystem dustVFX;

    Scoreboard scoreboard;
    Vector3 spawnLocation;
    Vector3 hitVfxAdjust = new Vector3(0f, 0.75f, 0f);

    int health = 2;
    int hitValue = 0;
    int deathValue = 2;

    void Start()
    {
        scoreboard = FindObjectOfType<Scoreboard>();
    }

    //when an arrow collides with this object, deplete health, increase score, play particle,
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
                Invoke(nameof(SpawnNewAbom), 2f);
                DisableAbom();
                Destroy(gameObject, 2f);
                Invoke(nameof(EnableAbom), 1.9999f);
            }
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        //emit small smoke cloud
        ParticleSystem vfx = Instantiate(dustVFX, transform.position + hitVfxAdjust, Quaternion.Euler(-90f, 0f, 0f));
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
        ParticleSystem vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = spawnAtRuntime.transform;
    }

    private void DetermineSpawnLocation()
    {
        Vector3 baseLocation = new Vector3(150f, 0f, 0f);
        Vector3 eugeneAdjustment = new Vector3(-14.02f, 2.05f, -6.12f);
        spawnLocation = baseLocation + eugeneAdjustment;
    }

    private void SpawnNewAbom()
    {
        GameObject newAbom = Instantiate(gameObject, spawnLocation, Quaternion.identity);
        newAbom.transform.parent = abominationsEmpty.transform;
        newAbom.name = "Eugene (Spawned)";
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
}
