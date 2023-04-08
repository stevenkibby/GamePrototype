using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragResetsPosition : MonoBehaviour, IDropHandler
{
    
    //CodeMonkey Drag/Drop video: https://www.youtube.com/watch?v=BGr-7GZJNXg

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        GameObject draggedItem = eventData.pointerDrag;
        if (draggedItem == null) { return; }

        ItemProperties draggedItemsProperties = draggedItem.GetComponent<ItemProperties>();
        ClickHandler draggedItemsDragHandlerScript = draggedItem.GetComponent<ClickHandler>();

        if (draggedItemsProperties.InterfaceType == InterfaceType.Inventory)
        {
            draggedItemsDragHandlerScript.SetParentInsideInventory(); //Sets dragged item's parent to InventoryItemParent
        }

        draggedItem.transform.position = draggedItemsDragHandlerScript.StartingPosition;
    }
}
