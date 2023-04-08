using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragToEquipment : MonoBehaviour, IDropHandler
{
    
    //CodeMonkey Drag/Drop video: https://www.youtube.com/watch?v=BGr-7GZJNXg

    [SerializeField] GameObject emptyEquipmentSlotIcon;
    [SerializeField] InventoryHandler inventoryHandlerScript;
    [SerializeField] EquippedType thisSlotsType;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        GameObject draggedItem = eventData.pointerDrag;
        if (draggedItem == null) { return; }
        
        ItemProperties draggedItemsProperties = draggedItem.GetComponent<ItemProperties>();
        ClickHandler draggedItemsDragHandlerScript = draggedItem.GetComponent<ClickHandler>();

        if (draggedItemsProperties.InterfaceType != InterfaceType.Inventory) //If dragged is Keybind or Equipped, just reset it to starting position
        {   
            draggedItem.transform.position = draggedItemsDragHandlerScript.StartingPosition;
            return;
        }

        EquippedType draggedItemsType = draggedItemsProperties.EquippedType;

        if (draggedItemsType != thisSlotsType) //Dragged = inventory - If wrong item type for this equipment slot, reset position and return
        {
            draggedItemsDragHandlerScript.SetParentInsideInventory(); //Sets dragged item's parent to InventoryItemParent
            draggedItem.transform.position = draggedItemsDragHandlerScript.StartingPosition;
            return;
        }

        else //Dragged = inventory and correct type.
        {
            draggedItem.transform.position = transform.position; //Move dragged object to this empty slot
            int index = inventoryHandlerScript.FindIndexOfEquippedSlot(thisSlotsType); //Gives you index for equipped slot of that type
            inventoryHandlerScript.SetEmptyEquippedIconsActive(index, false); //Disable that equipped slots empty icon
            inventoryHandlerScript.ProcessMovingToEmptySlot(int.Parse(name), draggedItem); //Update info to track which slots are now full.
            inventoryHandlerScript.UpdateKeybindActions(draggedItem, ActionType.Unequip); //Change keybind action if keybound
        }
    }
}

