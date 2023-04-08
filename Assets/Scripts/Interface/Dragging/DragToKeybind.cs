using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragToKeybind : MonoBehaviour, IDropHandler
{
    
    //CodeMonkey Drag/Drop video: https://www.youtube.com/watch?v=BGr-7GZJNXg

    [SerializeField] InventoryHandler inventoryHandlerScript;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        GameObject draggedItem = eventData.pointerDrag;
        if (draggedItem == null) { return; }
            
        ItemProperties draggedItemsProperties = draggedItem.GetComponent<ItemProperties>();
        int index = int.Parse(name);

        if (draggedItemsProperties.InterfaceType != InterfaceType.Keybind) //--- Dragging Inventory/Equipment to Empty Keybind ---
        {
            ClickHandler draggedItemsDragHandlerScript = draggedItem.GetComponent<ClickHandler>();
            Vector2 draggedPrefabsStartingPosition = draggedItemsDragHandlerScript.StartingPosition;
            
            GameObject newKeybind = inventoryHandlerScript.CreateKeybind(draggedItem, index, gameObject);
            inventoryHandlerScript.ModifyKeybindArray(index, true, newKeybind);
        
            if (draggedItemsProperties.InterfaceType == InterfaceType.Inventory)
            {
                draggedItemsDragHandlerScript.SetParentInsideInventory(); //Sets dragged item's parent to InventoryItemParent
            }

            draggedItem.transform.position = draggedPrefabsStartingPosition;
        }

        else //Else, --- dragging Keybind to Empty Keybind: ---
        {
            int previousIndex = draggedItemsProperties.CurrentSlotIndex;
            int updatedIndex = index;

            if (previousIndex != updatedIndex) //If not dragging onto itself
            {
                draggedItemsProperties.ModifyCurrentSlotIndex(index);
                inventoryHandlerScript.ModifyKeybindArray(previousIndex, false, draggedItem); //Possible bug - Index out of range exception - Click axe > Drag equipped axe to keybind
                inventoryHandlerScript.ModifyKeybindArray(updatedIndex, true, draggedItem);
            }
            
            draggedItem.transform.position = transform.position;
        }
        
    }
}
