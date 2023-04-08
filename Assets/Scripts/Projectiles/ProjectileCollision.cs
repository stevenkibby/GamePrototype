using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    //Properties
    bool hasImpaled = false;
    public bool HasImpaled { get { return hasImpaled; } }

    bool creatureWasHit = false;
    public bool CreatureWasHit { get { return creatureWasHit; } }
    
    GameObject hitObject;
    public GameObject HitObject { get { return hitObject; } }

    //Variables
    Rigidbody arrowRigidbody;
    CapsuleCollider arrowCollider;
    Quaternion arrowRotation = Quaternion.Euler(90f, 0f, 0f);
    TrailRenderer arrowTrail;
    ProjectilePartPool projectilePartPoolScript;

    void Awake()
    {
        projectilePartPoolScript = GetComponent<ProjectilePartPool>();
        arrowRigidbody = GetComponent<Rigidbody>();
        arrowCollider = GetComponent<CapsuleCollider>();
        arrowTrail = GetComponentInChildren<TrailRenderer>(); 
    }

    void OnEnable()
    {
        DisableArrowInteractions();
        ResetArrow();
    }

    void OnCollisionEnter(Collision other)
    {
        hitObject = other.gameObject;

        if ((hitObject.tag ==  "Ground") || 
            (hitObject.tag ==  "Tree"))
        {
            hasImpaled = true;
            arrowTrail.emitting = false;
            HitSoftObject(other);
            projectilePartPoolScript.EnablePartInPool(projectilePartPoolScript.dustPool);
        }

        else if ((hitObject.tag ==  "Phillip") || 
                 (hitObject.tag ==  "Eugene") || 
                 (hitObject.tag ==  "Mildred"))
        {
            hasImpaled = true;
            creatureWasHit = true;
            arrowTrail.emitting = false;
            HitSoftObject(other);
            projectilePartPoolScript.EnablePartInPool(projectilePartPoolScript.bloodPool);
        }

        else
        {
            //Hits hard object and just bounces off with physics
            projectilePartPoolScript.EnablePartInPool(projectilePartPoolScript.rockPool);
        }
    }

    void DisableArrowInteractions()
    {
        arrowRigidbody.isKinematic = true;
        arrowCollider.enabled = false;
    }

    void ResetArrow()
    {
        hasImpaled = false;
        creatureWasHit = false;
        hitObject = null;
        arrowTrail.emitting = false;
        arrowTrail.Clear(); //fixed so many bugs :)
    }

    void HitSoftObject(Collision other)
    {
        FreezeArrowVelocity();
        DisableArrowInteractions();
        gameObject.transform.SetParent(other.transform);
    }

    void FreezeArrowVelocity()
    {
        arrowRigidbody.velocity = Vector3.zero;
    }

    void TurnEmissionOff()
    {
        arrowTrail.emitting = false;
    }

    //FOR ARROW ARCING:
    //Arrow's acceleration in the local Y position effects arrows local rotation over the X axis!
    //Maybe not needed?
}
