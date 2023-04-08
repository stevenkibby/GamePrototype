using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSwapping : MonoBehaviour, IDropHandler
{
    
    //CodeMonkey Drag/Drop video: https://www.youtube.com/watch?v=BGr-7GZJNXg

    ItemProperties thisItemsProperties;
    ClickHandler thisItemsClickHandlerScript;
    InventoryHandler inventoryHandlerScript;

    void Awake()
    {
        thisItemsProperties = gameObject.GetComponent<ItemProperties>();
        thisItemsClickHandlerScript = gameObject.GetComponent<ClickHandler>();
        inventoryHandlerScript = GameObject.FindWithTag("Inventory").GetComponent<InventoryHandler>();
    }

    public void OnDrop(PointerEventData eventData) //When you release a dragged item over this game object
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        GameObject draggedItem = eventData.pointerDrag;
        if (draggedItem == null) { return; }

        ItemProperties draggedItemsProperties = draggedItem.GetComponent<ItemProperties>();
        InterfaceType draggedItemsInterfaceType = draggedItemsProperties.InterfaceType;
        InterfaceType thisItemsInterfaceType = thisItemsProperties.InterfaceType;

        if (draggedItemsInterfaceType == InterfaceType.Inventory)
        {
            if (thisItemsInterfaceType == InterfaceType.Inventory) //Inventory to Inventory Swap
            {
                SwapInventoryToInventory(draggedItem, draggedItemsProperties);
                return;
            }

            else if (thisItemsInterfaceType == InterfaceType.Equipped) //Inventory to Equipped Swap
            {
                if (draggedItemsProperties.EquippedType != thisItemsProperties.EquippedType)
                {
                    ResetDraggedItemsPosition(draggedItem);
                    inventoryHandlerScript.SetParentInsideInventory(draggedItem);
                    return;
                }

                SwapInventoryToEquipped(draggedItem, draggedItemsProperties);
                return;
            }

            else if (thisItemsInterfaceType == InterfaceType.Keybind) //Inventory to Keybind Swap
            {
                SwapInventoryToKeybind(draggedItem);
                return;
            }

            return;
        }
        
        else if (draggedItemsInterfaceType == InterfaceType.Equipped) 
        {
            if (thisItemsInterfaceType == InterfaceType.Inventory) //Equipped to Inventory Swap
            {
                if (draggedItemsProperties.EquippedType != thisItemsProperties.EquippedType)
                {
                    ResetDraggedItemsPosition(draggedItem);
                    return;
                }

                SwapEquippedToInventory(draggedItem, draggedItemsProperties);
                return;
            }

            else if (thisItemsInterfaceType == InterfaceType.Equipped) //Equipped to Equipped Swap
            {
                //Maybe adjust for dual wield eventually
                ResetDraggedItemsPosition(draggedItem);
                return;
            }

            else if (thisItemsInterfaceType == InterfaceType.Keybind) //Equipped to Keybind Swap
            {
                SwapEquippedToKeybind(draggedItem);
                return;
            }

            return;
        }

        else if (draggedItemsInterfaceType == InterfaceType.Keybind)
        {
            if (thisItemsInterfaceType == InterfaceType.Inventory) //Keybind to Inventory Swap
            {
                ResetDraggedItemsPosition(draggedItem);
                return;
            }

            else if (thisItemsInterfaceType == InterfaceType.Equipped) //Keybind to Equipped Swap
            {
                ResetDraggedItemsPosition(draggedItem);
                return;
            }

            else if (thisItemsInterfaceType == InterfaceType.Keybind) //Keybind to Keybind Swap
            {
                SwapKeybindToKeybind(draggedItem, draggedItemsProperties);
                return;
            }

            return;
        } 
    }

    void ResetDraggedItemsPosition(GameObject draggedItem)
    {
        Vector2 draggedItemsStartingPosition = draggedItem.GetComponent<ClickHandler>().StartingPosition;
        draggedItem.transform.position = draggedItemsStartingPosition;
    }

    void SwapInventoryToInventory(GameObject draggedItem, ItemProperties draggedItemsProperties)
    {
        ClickHandler draggedItemsClickHandlerScript = draggedItem.GetComponent<ClickHandler>();
        Vector2 draggedItemsStartingPosition = draggedItemsClickHandlerScript.StartingPosition;
        int draggedIndex = draggedItemsProperties.CurrentSlotIndex;
        int thisIndex = thisItemsProperties.CurrentSlotIndex;

        draggedItem.transform.position = transform.position; //Other object's position is now this ones
        transform.position = draggedItemsStartingPosition; //This position is now other object's old position
        thisItemsClickHandlerScript.UpdateStartingPosition(); //draggedPrefab will NOT update unless actually dragged!
        draggedItemsClickHandlerScript.UpdateStartingPosition();

        draggedItemsClickHandlerScript.SetParentInsideInventory();

        draggedItemsProperties.ModifyCurrentSlotIndex(thisIndex); //Swap the index property of both
        thisItemsProperties.ModifyCurrentSlotIndex(draggedIndex);

        inventoryHandlerScript.ModifyInventoryArray(draggedIndex, true, gameObject); //Swap the GameObject[] of both
        inventoryHandlerScript.ModifyInventoryArray(thisIndex, true, draggedItem);
    }

    //Inventory to equipped swap: draggedItem = inventory | gameObject = equipped
    public void SwapInventoryToEquipped(GameObject draggedItem, ItemProperties draggedItemsProperties)
    {
        ClickHandler draggedItemsClickHandlerScript = draggedItem.GetComponent<ClickHandler>();
        Vector2 draggedItemsStartingPosition = draggedItemsClickHandlerScript.StartingPosition;
        int draggedIndex = draggedItemsProperties.CurrentSlotIndex;
        int thisIndex = thisItemsProperties.CurrentSlotIndex;

        draggedItem.transform.position = transform.position; //Other object's position is now this ones
        transform.position = draggedItemsStartingPosition; //This position is now other object's old position
        thisItemsClickHandlerScript.UpdateStartingPosition(); 
        draggedItemsClickHandlerScript.UpdateStartingPosition(); //draggedPrefab will NOT update unless actually dragged!

        thisItemsClickHandlerScript.SetParentInsideInventory();
        draggedItemsClickHandlerScript.SetParentOutsideInventory();

        draggedItemsProperties.ModifyCurrentSlotIndex(thisIndex); //Swap the index property of both
        thisItemsProperties.ModifyCurrentSlotIndex(draggedIndex);

        inventoryHandlerScript.ModifyInventoryArray(draggedIndex, true, gameObject); //Swap the GameObject[] of both
        inventoryHandlerScript.ModifyEquippedArray(thisIndex - 100, true, draggedItem); 

        thisItemsProperties.SetInterfaceType(InterfaceType.Inventory);
        draggedItemsProperties.SetInterfaceType(InterfaceType.Equipped);

        thisItemsProperties.SetActionType(ActionType.Equip);
        draggedItemsProperties.SetActionType(ActionType.Unequip);

        inventoryHandlerScript.UpdateKeybindActions(gameObject, ActionType.Equip); //Swap keybind actions if keybound
        inventoryHandlerScript.UpdateKeybindActions(draggedItem, ActionType.Unequip);
    }

    void SwapInventoryToKeybind(GameObject draggedItem)
    {
        ClickHandler draggedItemsDragHandlerScript = draggedItem.GetComponent<ClickHandler>();
        Vector2 draggedItemsStartingPosition = draggedItemsDragHandlerScript.StartingPosition;
        draggedItem.transform.position = draggedItemsStartingPosition; //return dragged item to starting position
        draggedItemsDragHandlerScript.SetParentInsideInventory(); //Sets dragged item's parent to InventoryItemParent

        if (draggedItem.name == gameObject.name) //If keybind is the same item you're trying to drag in, don't create new one.
        {
            Debug.Log("Keybind is the same as the item we're trying to drag in");
            return;
        }

        int index = thisItemsProperties.CurrentSlotIndex;
        GameObject newKeybind = inventoryHandlerScript.CreateKeybind(draggedItem, index, gameObject);
        inventoryHandlerScript.ModifyKeybindArray(index, true, newKeybind);
        Destroy(gameObject);
    }

    //Equipped to Inventory swap: draggedItem = equipped | gameObject = inventory
    public void SwapEquippedToInventory(GameObject draggedItem, ItemProperties draggedItemsProperties)
    {
        ClickHandler draggedItemsClickHandlerScript = draggedItem.GetComponent<ClickHandler>();
        Vector2 draggedItemsStartingPosition = draggedItemsClickHandlerScript.StartingPosition;
        int draggedIndex = draggedItemsProperties.CurrentSlotIndex;
        int thisIndex = thisItemsProperties.CurrentSlotIndex;

        draggedItem.transform.position = transform.position; //Other object's position is now this ones
        transform.position = draggedItemsStartingPosition; //This position is now other object's old position
        thisItemsClickHandlerScript.UpdateStartingPosition(); 
        draggedItemsClickHandlerScript.UpdateStartingPosition(); //draggedPrefab will NOT update unless actually dragged!

        draggedItemsClickHandlerScript.SetParentInsideInventory(); //dragged needs parent set to InventoryItemParent
        thisItemsClickHandlerScript.SetParentOutsideInventory(); //this item needs parent set to OtherItemParent

        draggedItemsProperties.ModifyCurrentSlotIndex(thisIndex); //Swap the index property of both
        thisItemsProperties.ModifyCurrentSlotIndex(draggedIndex);

        inventoryHandlerScript.ModifyInventoryArray(thisIndex, true, draggedItem); //Swap the GameObject[] of both
        inventoryHandlerScript.ModifyEquippedArray(draggedIndex - 100, true, gameObject);

        thisItemsProperties.SetInterfaceType(InterfaceType.Equipped);
        draggedItemsProperties.SetInterfaceType(InterfaceType.Inventory);

        thisItemsProperties.SetActionType(ActionType.Unequip);
        draggedItemsProperties.SetActionType(ActionType.Equip);

        inventoryHandlerScript.UpdateKeybindActions(gameObject, ActionType.Unequip); //Swap keybind actions if keybound
        inventoryHandlerScript.UpdateKeybindActions(draggedItem, ActionType.Equip);
    }

    void SwapEquippedToKeybind(GameObject draggedItem)
    {
        ClickHandler draggedItemsDragHandlerScript = draggedItem.GetComponent<ClickHandler>();
        Vector2 draggedItemsStartingPosition = draggedItemsDragHandlerScript.StartingPosition;
        draggedItem.transform.position = draggedItemsStartingPosition; //return dragged item to starting position
        
        if (draggedItem.name == gameObject.name) //If keybind is the same item you're trying to drag in, don't create new one.
        {
            Debug.Log("Keybind is the same as the item we're trying to drag in");
            return;
        }

        int index = thisItemsProperties.CurrentSlotIndex;
        GameObject newKeybind = inventoryHandlerScript.CreateKeybind(draggedItem, index - 100, gameObject);
        inventoryHandlerScript.ModifyKeybindArray(index, true, newKeybind);
        Destroy(gameObject);
    }

    void SwapKeybindToKeybind(GameObject draggedItem, ItemProperties draggedItemsProperties)
    {
        if (draggedItem.name == gameObject.name) //Dragging onto another of the same item, more optimized
        {
            ResetDraggedItemsPosition(draggedItem);
        }

        ClickHandler draggedItemsClickHandlerScript = draggedItem.GetComponent<ClickHandler>();
        Vector2 draggedItemsStartingPosition = draggedItemsClickHandlerScript.StartingPosition;
        int draggedIndex = draggedItemsProperties.CurrentSlotIndex;
        int thisIndex = thisItemsProperties.CurrentSlotIndex;

        draggedItem.transform.position = transform.position; //Other object's position is now this ones
        transform.position = draggedItemsStartingPosition; //This position is now other object's old position
        thisItemsClickHandlerScript.UpdateStartingPosition(); 
        draggedItemsClickHandlerScript.UpdateStartingPosition(); //draggedPrefab will NOT update unless actually dragged!

        draggedItemsProperties.ModifyCurrentSlotIndex(thisIndex); //Swap the index property of both
        thisItemsProperties.ModifyCurrentSlotIndex(draggedIndex);

        inventoryHandlerScript.ModifyKeybindArray(draggedIndex, true, gameObject); //Swap the GameObject[] of both
        inventoryHandlerScript.ModifyKeybindArray(thisIndex, true, draggedItem);
    }
}
