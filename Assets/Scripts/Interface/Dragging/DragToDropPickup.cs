using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragToDropPickup : MonoBehaviour, IDropHandler
{
    
    //CodeMonkey Drag/Drop video: https://www.youtube.com/watch?v=BGr-7GZJNXg

    [SerializeField] InventoryHandler inventoryHandlerScript;
    [SerializeField] DropPhysicalPickup dropPhysicalPickupScript;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        GameObject draggedItem = eventData.pointerDrag;
        if (draggedItem == null) { return; }
        
        ItemProperties draggedItemsProperties = draggedItem.GetComponent<ItemProperties>();

        if (draggedItemsProperties.EquippedType == EquippedType.Bracelet) //if trying to drop a bracelet
        {
            ClickHandler draggedItemsDragHandlerScript = draggedItem.GetComponent<ClickHandler>();
            draggedItem.transform.position = draggedItemsDragHandlerScript.StartingPosition;
            Debug.Log("You shouldn't drop your bracelet!");
            return;
        }

        if (draggedItemsProperties.InterfaceType != InterfaceType.Keybind)
        {
            dropPhysicalPickupScript.DropPickup(draggedItem, draggedItemsProperties.Count);
        }
        
        inventoryHandlerScript.RemoveItem(draggedItem, draggedItemsProperties);
    }
}
