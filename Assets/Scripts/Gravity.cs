using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] float risingGravityScale = 3f;
    [SerializeField] float fallingGravityScale = 6f;
    float globalGravity = -9.81f;
    Rigidbody rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        if(rb.velocity.y >= 0)
        {
            Vector3 gravity = (globalGravity * Vector3.up * risingGravityScale);
            rb.AddForce(gravity, ForceMode.Acceleration);
        }

        if(rb.velocity.y <= 0)
        {
            Vector3 gravity = (globalGravity * Vector3.up * fallingGravityScale);
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }
}
