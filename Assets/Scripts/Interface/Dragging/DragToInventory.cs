using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragToInventory : MonoBehaviour, IDropHandler
{
    
    //CodeMonkey Drag/Drop video: https://www.youtube.com/watch?v=BGr-7GZJNXg

    [SerializeField] InventoryHandler inventoryHandlerScript;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        GameObject draggedItem = eventData.pointerDrag;
        if (draggedItem == null) { return; }
        
        ItemProperties draggedItemsProperties = draggedItem.GetComponent<ItemProperties>();

        if (draggedItemsProperties.InterfaceType == InterfaceType.Keybind) //If dragged is Keybind, just reset it to starting position
        {   
            ClickHandler draggedItemsDragHandlerScript = draggedItem.GetComponent<ClickHandler>();
            draggedItem.transform.position = draggedItemsDragHandlerScript.StartingPosition;
            return;
        }

        draggedItem.transform.position = transform.position; //Move dragged object to this empty slot
        inventoryHandlerScript.ProcessMovingToEmptySlot(int.Parse(name), draggedItem); //Update bool arrays to track which slots are now full.
        inventoryHandlerScript.UpdateKeybindActions(draggedItem, ActionType.Equip); //Change keybind action if keybound
    }
}
