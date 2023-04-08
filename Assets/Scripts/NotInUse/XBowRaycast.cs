using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XBowRaycast : MonoBehaviour
{
    [SerializeField] Camera TPCamera;
    [SerializeField] float range = 100f;
    [SerializeField] InputActionReference attack;

    [SerializeField] float crossbowDamage = 1f;

    void Update()
    {
        if (attack.action.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        //play crossbow shoot effect
        ProcessRaycast();
    }

    void ProcessRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(TPCamera.transform.position, TPCamera.transform.forward, out hit, range))
        {
            Debug.Log($"You hit {hit.transform.name} with a crossbow bolt!");
            //TODO: add hit effect

            ElementalHealth target = hit.transform.GetComponent<ElementalHealth>();

            if (target == null) { return; }

            target.TakeDamage(crossbowDamage);
            //call a method on EnemyHealth that decreases the enemy's health.
        }

        else
        {
            return;
        }
    }
}
