using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElemAttack : MonoBehaviour
{

    PlayerHealth target;
    [SerializeField] float damage = 10f;

    void Start()
    {
        target = FindObjectOfType<PlayerHealth>();
    }

    public void AttackHitEvent()
    {
        if (target == null) { return; }
        target.TakeDamage(damage);
        Debug.Log("Elemental attacked");
    }

    void OnDamageTaken()
    {
        Debug.Log("OnDamageTaken was broadcasted");
    }
}
