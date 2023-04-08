using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisable : MonoBehaviour
{
    [SerializeField] InventoryHandler inventoryHandlerScript;

    void OnDisable()
    {
        if ((inventoryHandlerScript.IsDragging) && (inventoryHandlerScript.DraggedItem != null))
        {
            GameObject draggedItem = inventoryHandlerScript.DraggedItem;
            ItemProperties draggedItemsProperties = inventoryHandlerScript.DraggedItem.GetComponent<ItemProperties>();

            if (draggedItemsProperties.EquippedType == EquippedType.Bracelet) //Dragging first bracelet
            {
                ClickHandlerNewBracelet draggedItemsClickHandlerNewBraceletScript = inventoryHandlerScript.DraggedItem.GetComponent<ClickHandlerNewBracelet>();
                inventoryHandlerScript.SetParentInsideInventory(draggedItem);
                draggedItemsClickHandlerNewBraceletScript.DisableCurrentDrag();
                return;
            }

            ClickHandler draggedItemsClickHandlerScript = inventoryHandlerScript.DraggedItem.GetComponent<ClickHandler>();
            
            if (draggedItemsProperties.InterfaceType == InterfaceType.Inventory)
            {
                inventoryHandlerScript.SetParentInsideInventory(draggedItem);
            }

            draggedItemsClickHandlerScript.DisableCurrentDrag();
        }
    }
}
