using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalHealth : MonoBehaviour
{
    [SerializeField] float health = 100f;

    public void TakeDamage(float damage)
    {
        BroadcastMessage("OnDamageTaken");
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        Debug.Log(health);
    }
}
