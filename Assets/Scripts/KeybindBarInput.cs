using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindBarInput : MonoBehaviour
{

    [SerializeField] InventoryHandler inventoryHandlerScript;

    void Update()
    {
        if (Input.anyKeyDown) //Only does 1 check per frame instead of 8. Called the frame any key is pressed
        {
            ProcessKeyInput();
        }
    }

    void ProcessKeyInput() // Keybind bar Keys 1-8
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ProcessKeybind(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ProcessKeybind(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ProcessKeybind(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ProcessKeybind(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ProcessKeybind(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ProcessKeybind(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ProcessKeybind(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ProcessKeybind(7);
        }
    }

    void ProcessKeybind(int value)
    {
        GameObject keybind = inventoryHandlerScript.ReturnKeybindItemOfIndex(value);
        if (keybind == null) { return; }

        ActionType keybindsActionType = keybind.GetComponent<ItemProperties>().ActionType;

        if (keybindsActionType == ActionType.NoAction) // --- Pressing Keybind with Action: No Action ---
        {
            //maybe play small animation 
            return;
        }

        else if (keybindsActionType == ActionType.Equip) // --- Pressing Keybind with Action: Equip ---
        {
            GameObject keyboundItem = inventoryHandlerScript.FindSameItemInInventory(keybind);

            if (keyboundItem == null)
            {
                Debug.Log(keybind + " is not currently in your inventory.");
                return;
            }

            inventoryHandlerScript.SetParentOutsideInventory(keyboundItem);
            ItemProperties keyboundItemsProperties = keyboundItem.GetComponent<ItemProperties>();
            keyboundItem.GetComponent<ClickHandler>().EquipItem(keyboundItem, keyboundItemsProperties);
            return;
        }

        else if (keybindsActionType == ActionType.Unequip) // --- Pressing Keybind with Action: Unequip ---
        {
            GameObject keyboundItem = inventoryHandlerScript.FindSameItemInEquipped(keybind);

            if (keyboundItem == null)
            {
                Debug.Log(keybind + " is not currently equipped.");
                return;
            }

            ItemProperties keyboundItemsProperties = keyboundItem.GetComponent<ItemProperties>();
            keyboundItem.GetComponent<ClickHandler>().UnequipItem(keyboundItem, keyboundItemsProperties);
            return;
        }

        else if (keybindsActionType == ActionType.Consume) // --- Pressing Keybind with Action: Consume ---
        {
            GameObject keyboundItem = inventoryHandlerScript.FindSameItemInInventory(keybind);

            if (keyboundItem == null)
            {
                Debug.Log(gameObject + " is not currently in your inventory.");
                return;
            }

            ItemProperties keyboundItemsProperties = keyboundItem.GetComponent<ItemProperties>();
            inventoryHandlerScript.ConsumeItem(keyboundItem, keyboundItemsProperties);
            Debug.Log("Consumed: " + keyboundItem);
            
            //consume item:
            //Subtract 1 from quantity, if 0 destroy it.
            //Increase health or stamina once system is in place
            //Start Coroutine to eat or drink (Consumes immediately, but delays ability to consume again)
            return;
        }
    }
    
}
