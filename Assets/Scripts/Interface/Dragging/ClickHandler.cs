using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    //CodeMonkey Drag/Drop video: https://www.youtube.com/watch?v=BGr-7GZJNXg

    Canvas tabCanvas;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    InventoryHandler inventoryHandlerScript;
    GameObject temporaryClone;

    Vector2 startingPosition;
    public Vector2 StartingPosition { get { return startingPosition; } }

    bool hasBeenDragged = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        tabCanvas = GameObject.FindWithTag("TabCanvas").GetComponent<Canvas>();

        inventoryHandlerScript = GameObject.FindWithTag("Inventory").GetComponent<InventoryHandler>();
    }

    public void OnPointerDown(PointerEventData eventData) //Triggers if left/middle/right button was clicked over this object
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        
        UpdateStartingPosition();
        canvasGroup.alpha = .5f; //The permanent transparency of the clone
        canvasGroup.blocksRaycasts = false; //raycast goes through this object and lands on object below it like item slot
        inventoryHandlerScript.SetParentOutsideInventory(gameObject); //Sets it's parent to OtherItemParent
        transform.SetAsLastSibling(); //so dragged image always appears above inventory images
        
        if (temporaryClone == null)
        {
            temporaryClone = Instantiate(gameObject, gameObject.transform.parent);
            Color temporaryClonesColor = new Color(0f, 0f, 0f, 1f);
            temporaryClone.transform.Find("Image").GetComponent<Image>().color = temporaryClonesColor; //Set color of temporaryClone to look like a shadow
            Transform temporaryClonesText = temporaryClone.transform.Find("Text");

            if (temporaryClonesText != null)
            {
                temporaryClonesText.GetComponent<TMP_Text>().enabled = false; //Disable clone's text if parent has text enabled
            }
        }

        else //temporaryClone exists but was disabled
        {
            temporaryClone.SetActive(true);
        }

        temporaryClone.transform.position = transform.position;
        temporaryClone.transform.SetAsFirstSibling();
        canvasGroup.alpha = .5f; //The transparency of the dragged item
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        canvasGroup.alpha = 1f; //returns to no transparency on release
        canvasGroup.blocksRaycasts = true; //Can be interacted with again
        temporaryClone.SetActive(false);

        if (!hasBeenDragged) //------- Releasing left mouse without a drag - AKA clicking - Action or nothing/small animation if no action -------
        {
            ItemProperties itemsProperties = gameObject.GetComponent<ItemProperties>();
            ActionType itemsActionType = itemsProperties.ActionType;
            InterfaceType itemsInterfaceType = itemsProperties.InterfaceType;

            if (itemsInterfaceType == InterfaceType.Equipped) //----------- If it's equipped, only possible ActionType is Unequip
            {
                UnequipItem(gameObject, itemsProperties);
                return;
            }

            else if (itemsInterfaceType == InterfaceType.Inventory) //------------ If it's inventory, can be NoAction, Equip, or Consume
            {
                if (itemsActionType == ActionType.NoAction)
                {
                    //maybe play small animation
                    SetParentInsideInventory();
                    return;
                }

                else if (itemsActionType == ActionType.Equip)
                {
                    EquipItem(gameObject, itemsProperties); //Equips this item //BUG HERE *************************************************************************************************************
                    return;
                }

                else if (itemsActionType == ActionType.Consume)
                {
                    Debug.Log("Consumed: " + gameObject);
                    //consume item:
                    //Subtract 1 from quantity, if 0 destroy it.
                    SetParentInsideInventory();
                    inventoryHandlerScript.ConsumeItem(gameObject, itemsProperties);
                    //Increase health or stamina once system is in place
                    //Start Coroutine to eat or drink (Consumes immediately, but delays ability to consume again)
                    return;
                }
            }

            else if (itemsInterfaceType == InterfaceType.Keybind) //----------- If it's keybind, can be NoAction, Equip, Unequip, or Consume
            {
                if (itemsActionType == ActionType.NoAction) // --- Pressing Keybind with Action: No Action ---
                {
                    //maybe play small animation
                    return;
                }

                else if (itemsActionType == ActionType.Equip) // --- Pressing Keybind with Action: Equip ---
                {
                    GameObject keyboundItem = inventoryHandlerScript.FindSameItemInInventory(gameObject);

                    if (keyboundItem == null)
                    {
                        Debug.Log(gameObject + " is not currently in your inventory.");
                        return;
                    }

                    inventoryHandlerScript.SetParentOutsideInventory(keyboundItem);
                    ItemProperties keyboundItemsProperties = keyboundItem.GetComponent<ItemProperties>();
                    EquipItem(keyboundItem, keyboundItemsProperties);
                    return;
                }

                else if (itemsActionType == ActionType.Unequip) // --- Pressing Keybind with Action: Unequip ---
                {
                    GameObject keyboundItem = inventoryHandlerScript.FindSameItemInEquipped(gameObject);

                    if (keyboundItem == null)
                    {
                        Debug.Log(gameObject + " is not currently equipped.");
                        return;
                    }

                    ItemProperties keyboundItemsProperties = keyboundItem.GetComponent<ItemProperties>();
                    UnequipItem(keyboundItem, keyboundItemsProperties);
                    return;
                }

                else if (itemsActionType == ActionType.Consume) // --- Pressing Keybind with Action: Consume ---
                {
                    GameObject keyboundItem = inventoryHandlerScript.FindSameItemInInventory(gameObject);

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
    }

    public void OnBeginDrag(PointerEventData evenData)
    {
        hasBeenDragged = true;
        inventoryHandlerScript.ItemIsDragging(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        rectTransform.anchoredPosition += eventData.delta / tabCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        UpdateStartingPosition();
        canvasGroup.alpha = 1f; //returns to no transparency on release
        canvasGroup.blocksRaycasts = true; //Can be interacted with again
        hasBeenDragged = false;
        inventoryHandlerScript.ItemIsNotDragging();
        temporaryClone.SetActive(false);
    }

    public void UpdateStartingPosition()
    {
        startingPosition = transform.position;
    }

    public void SetParentInsideInventory()
    {
        inventoryHandlerScript.SetParentInsideInventory(gameObject);
    }

    public void SetParentOutsideInventory()
    {
        inventoryHandlerScript.SetParentOutsideInventory(gameObject);
    }

    public void DisableCurrentDrag()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        transform.position = startingPosition;
        temporaryClone.SetActive(false);
    }
    
    void OnDestroy() //Destroy temporaryClone if this object is also destroyed
    {
        if (temporaryClone != null)
        {
            Destroy(temporaryClone);
        }
    }

    public void UnequipItem(GameObject item, ItemProperties itemsProperties)
    {
        if (itemsProperties.EquippedType == EquippedType.Quiver) //If it's quiver, and that same item is in inventory, just add it to the item instead
        {
            GameObject inventoryItem = inventoryHandlerScript.FindSameItemInInventory(item); //Find first object in inventory of this gameObject.
            
            if (inventoryItem != null)
            {
                ItemProperties inventoryItemsProperties = inventoryItem.GetComponent<ItemProperties>();
                inventoryItemsProperties.ModifyCount(itemsProperties.Count);
                inventoryHandlerScript.UpdateKeybindActions(gameObject, ActionType.Equip); //Updates keybinds to equip action type
                inventoryHandlerScript.RemoveItem(item, itemsProperties);
                return;
            }
        }

        if (itemsProperties.EquippedType == EquippedType.Bracelet) //if trying to unequip a bracelet
        {
            Debug.Log("Unequipping your bracelet is too dangerous!");
            return;
        }

        int firstEmptySlot = inventoryHandlerScript.FindFirstEmptySlot();

        if (inventoryHandlerScript.InventoryIsFull)
        {
            Debug.Log("Inventory is full.");
            return;
        }

        inventoryHandlerScript.MoveToEmptyInventorySlot(firstEmptySlot, item);
        inventoryHandlerScript.ProcessMovingToEmptySlot(firstEmptySlot, item);
        inventoryHandlerScript.UpdateKeybindActions(gameObject, ActionType.Equip); //Updates keybinds to equip action type
    }

    //dragged = itemToEquip, EquippedItem = equipped item. Get ItemSwapping Component of equippedItem. Call Inventory to Equipped Swap.
    public void EquipItem(GameObject itemToEquip, ItemProperties itemsProperties) //Item to equip, that item's properties 
    {
        EquippedType itemToEquipsEquippedType = itemsProperties.EquippedType;
        GameObject equippedItem = inventoryHandlerScript.FindItemOfTypeInEquipped(itemToEquipsEquippedType);
        
        if (equippedItem != null) //found an item of that type in Equipped, so equipped slot is full
        {
            ItemProperties equippedItemsProperties = equippedItem.GetComponent<ItemProperties>();

            if ((itemToEquipsEquippedType == EquippedType.Quiver) && (itemToEquip.name == equippedItem.name)) //If they're both quiver type and the same item
            {
                equippedItemsProperties.ModifyCount(itemsProperties.Count);
                inventoryHandlerScript.UpdateKeybindActions(gameObject, ActionType.Unequip); //Updates keybinds to unequip action type
                inventoryHandlerScript.RemoveItem(itemToEquip, itemsProperties);
                return;
            }

            else //Swap itemToEquip with equippedItem
            {
                equippedItem.GetComponent<ItemSwapping>().SwapInventoryToEquipped(itemToEquip, itemsProperties); //Swap Items
                inventoryHandlerScript.UpdateKeybindActions(equippedItem, ActionType.Equip);
                inventoryHandlerScript.UpdateKeybindActions(itemToEquip, ActionType.Unequip);
                return;
            }
        }

        else //didn't find an item of that type in Equipped, so equipped slot is empty and you can just move to it
        {
            int index = inventoryHandlerScript.FindIndexOfEquippedSlot(itemToEquipsEquippedType); //Finds the equipped index of the type of the item BUG: 2
            inventoryHandlerScript.MoveToEmptyEquippedSlot(index, itemToEquip); //Moves to gear and disables empty equipped icon
            inventoryHandlerScript.ProcessMovingToEmptySlot(index + 100, itemToEquip);
            inventoryHandlerScript.UpdateKeybindActions(gameObject, ActionType.Unequip); //Updates keybinds to unequip action type
            return;
        }
    }
}
