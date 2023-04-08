using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TabInputManager : MonoBehaviour
{
    [SerializeField] InputActionReference tab;
    [SerializeField] GameObject equipped;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject inventorySlots;
    [SerializeField] GameObject inventoryItemParent;
    [SerializeField] GameObject titles;
    [SerializeField] GameObject crosshair;
    [SerializeField] CameraController cameraControllerScript;
    [SerializeField] ArrowLauncher arrowLauncherScript;

    bool currentlyTabbed = false;
    public bool CurrentlyTabbed { get { return currentlyTabbed; } }
    
    // Update is called once per frame
    void Update()
    {
        if (tab.action.WasPressedThisFrame())
        {
            if (!currentlyTabbed)
            {
                cameraControllerScript.enabled = false; //disable camera controller script
                Cursor.lockState = CursorLockMode.None; //unlock cursor
                Cursor.visible = true; //can see cursor
                inventory.SetActive(true);
                inventorySlots.SetActive(true);
                inventoryItemParent.SetActive(true);
                titles.SetActive(true);
                crosshair.SetActive(false);
                arrowLauncherScript.UnnockArrow();
                currentlyTabbed = true;
                
            }

            else
            {
                cameraControllerScript.enabled = true; //enable camera controller script
                Cursor.lockState = CursorLockMode.Locked; //lock cursor
                Cursor.visible = false; //can't see cursor
                inventorySlots.SetActive(false);
                inventoryItemParent.SetActive(false);
                titles.SetActive(false);
                crosshair.SetActive(true);
                currentlyTabbed = false;
            }
        }
    }
}
