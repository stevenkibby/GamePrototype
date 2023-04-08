using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] InputActionReference mousePosition;

    [SerializeField] float mouseSensitivity = 250f;

    [SerializeField] Transform player;
    [SerializeField] Transform vertRotation;


    void Awake()
    {
       Cursor.lockState = CursorLockMode.Locked;
       Cursor.visible = false;
    }

    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        
        //Vector2 mousePos = mousePosition.action.ReadValue<Vector2>();
        //Using old unity input manager because value goes from 0 to +1 or -1 and then back. New one isn't resetting

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        player.Rotate(Vector3.up, mouseX);
        vertRotation.Rotate(Vector3.right, -mouseY);
        
    }
}
