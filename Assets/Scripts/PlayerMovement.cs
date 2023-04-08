using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //General variables
    [SerializeField] InputActionReference move, run, jump;
    Rigidbody rb;

    //Movespeed variables
    [SerializeField] float walkSpeed = 5.0f;
    [SerializeField] float runSpeed = 10.0f;
    float moveSpeed;

    //Jumping variables
    [SerializeField] float jumpFactor = 12f;
    bool isGrounded = false;
    bool recentlyJumped = false;
    float jumpDelay = 0.25f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Run();
        Jump();
    }
    
    void FixedUpdate()
    {
        Move();
        isGrounded = false;
    }

    void Run() //Input: Shift | Doubles move speed
    {
        if (run.action.IsPressed())
        {
            moveSpeed = runSpeed;
        }

        else
        {
            moveSpeed = walkSpeed;
        }
    }

    void Move() //Input: WASD | Moves player forward/backward/left/right
    {
        Vector2 inputVector = move.action.ReadValue<Vector2>();
        float horizontalValue = inputVector.x * moveSpeed;
        float verticalValue = inputVector.y * moveSpeed;
        Vector3 velocity = new Vector3(horizontalValue, rb.velocity.y, verticalValue); // Somehow takes camera direction into account
        rb.velocity = transform.TransformDirection(velocity); //Idk why lol
    }

    void Jump() //Input: Space bar | Player jumps
    {
        if ((isGrounded) && (jump.action.WasPressedThisFrame()) && (!recentlyJumped))
        {
            rb.AddForce(Vector3.up * jumpFactor, ForceMode.Impulse);
            isGrounded = false;
            StartCoroutine(RecentlyJumped());
        }
    }

    void OnTriggerStay(Collider other)
    {
        isGrounded = true;
    }

    IEnumerator RecentlyJumped()
    {
        recentlyJumped = true;
        yield return new WaitForSeconds(jumpDelay);
        recentlyJumped = false;
    }
}
