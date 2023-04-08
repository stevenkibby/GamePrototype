using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BraceletDragResetsPosition : MonoBehaviour, IDropHandler
{
    
    //CodeMonkey Drag/Drop video: https://www.youtube.com/watch?v=BGr-7GZJNXg

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        GameObject draggedItem = eventData.pointerDrag;
        if (draggedItem == null) { return; }

        ClickHandlerNewBracelet draggedItemsDragHandlerScript = draggedItem.GetComponent<ClickHandlerNewBracelet>();
        draggedItemsDragHandlerScript.SetParentInsideInventory(); //Sets dragged item's parent to InventoryItemParent
        draggedItem.transform.position = draggedItemsDragHandlerScript.StartingPosition;
        Debug.Log("Reset position");
    }
}
