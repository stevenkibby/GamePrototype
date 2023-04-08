using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryHandler : MonoBehaviour
{

    [SerializeField] GameObject inventorySlots;
    [SerializeField] GameObject equippedSlots;
    [SerializeField] Transform inventoryItemParent;
    [SerializeField] Transform outsideItemParent;
    [SerializeField] InterfaceCoroutines interfaceCoroutinesScript;
    [SerializeField] EquippedPhysicalItems playerEquipmentScript;
    [SerializeField] AddedItemNotification addedItemNotificationScript;
    [SerializeField] NewItemDiscovered newItemDiscoveredScript;
    [SerializeField] BoltLauncher boltLauncherScript; //Needs to change once you get two types of crossbows!
    [SerializeField] ArrowLauncher arrowLauncherScript; //same as above
    [SerializeField] BuffHandler buffHandlerScript;

    List<GameObject> itemsInInventoryList = new List<GameObject>();
    List<GameObject> activeBuffsList = new List<GameObject>();
    GameObject[] itemsInInventoryArray;
    GameObject[] itemsInEquippedArray;
    GameObject[] itemsInKeybindArray;
    

    [SerializeField] GameObject[] emptyEquippedIcons;
    EquippedType[] itemTypeOfEachEquippedSlot = {EquippedType.Cape, EquippedType.Helmet, EquippedType.Quiver, 
                                             EquippedType.Mainhand, EquippedType.Chest, EquippedType.Offhand, 
                                             EquippedType.Ring, EquippedType.Legs, EquippedType.Bracelet};

    int inventorySlotsCount = 32;
    int equippedSlotsCount = 9;
    int keybindSlotsCount = 8;
    bool lastItemWasRemoved = false;
    [SerializeField] GameObject startingBracelet;
    GameObject currentBracelet;

    GameObject draggedItem;
    public GameObject DraggedItem { get { return draggedItem; } }

    bool isDragging;
    public bool IsDragging { get { return isDragging; } }

    bool inventoryIsFull;
    public bool InventoryIsFull { get { return inventoryIsFull; } }

    void Awake()
    {
        itemsInInventoryArray = new GameObject[inventorySlotsCount];
        itemsInEquippedArray = new GameObject[equippedSlotsCount];
        itemsInKeybindArray = new GameObject[keybindSlotsCount];

        startingBracelet.SetActive(false);
        gameObject.SetActive(false);

        //BUG - If you add an item before opening inventory, item goes to 0,0 spot of inventory instead.
        //This fixes by disabling GridLayoutGroup for now. Otherwise Coroutine to wait until end of frame fixes it.
    }

    void OnDisable() //If you disable inventory while dragging, below avoids bugs with parenting.
    {
        if ((isDragging) && (draggedItem != null))
        {
            ItemProperties draggedItemsProperties = draggedItem.GetComponent<ItemProperties>();
            ClickHandler draggedItemsDragHandlerScript = draggedItem.GetComponent<ClickHandler>();

            if (draggedItemsProperties.InterfaceType == InterfaceType.Inventory)
            {
                draggedItem.transform.SetParent(inventoryItemParent);
            }

            draggedItemsDragHandlerScript.DisableCurrentDrag();
        }
    }

    public GameObject CheckForItem(GameObject item)
    {
        for (int i = 0; i < itemsInInventoryList.Count; i++)
        {
            if (itemsInInventoryList[i].name == item.name)
            {
                return itemsInInventoryList[i];
            }
        }

        return null;
    }

    public int FindFirstEmptySlot()
    {
        for (int i = 0; i < itemsInInventoryArray.Length; i++)
        {  
            if (itemsInInventoryArray[i] == null)
            {
                inventoryIsFull = false;
                return i;
            }
        }

        inventoryIsFull = true;
        return inventorySlotsCount; //Inventory is full
    }

    public bool IncreaseStackableItem(GameObject item, int count) //returns true if increased or created, false if not.
    {
        GameObject existingItem = CheckForItem(item);

        if (existingItem != null)
        {
            existingItem.GetComponent<ItemProperties>().ModifyCount(count);

            addedItemNotificationScript.gameObject.SetActive(true);
            Sprite itemSprite = addedItemNotificationScript.CreateAddedItemNotification(item, count, true); //Create Stackable "Added Item" Notification + store sprite

            if (!newItemDiscoveredScript.DiscoveredItems[item.name]) //If item has not yet been discovered:
            {
                newItemDiscoveredScript.gameObject.SetActive(true);
                newItemDiscoveredScript.CreateDiscoveredNotification(item.name, itemSprite);
            }

            return true;
        }

        return CreateItem(item, count);
    }

    public bool CreateItem(GameObject item, int count) //returns true if created, false if not.
    {
        int firstEmptySlot = FindFirstEmptySlot(); //finds first empty slot

        if (inventoryIsFull) //If Inventory is full:
        {
            Debug.Log("Inventory Is Full.");
            return false;
        }

        GameObject newItem = Instantiate(item, inventoryItemParent); //Instantiate the item
        newItem.name = item.name; //So you can CheckForItem()
        itemsInInventoryList.Add(newItem);
        itemsInInventoryArray[firstEmptySlot] = newItem;

        MoveToEmptyInventorySlot(firstEmptySlot, newItem);

        ItemProperties newItemProperties = newItem.GetComponent<ItemProperties>();
        newItemProperties.ModifyCurrentSlotIndex(firstEmptySlot);
        newItemProperties.ModifyCount(count); //Change the count of the item

        addedItemNotificationScript.gameObject.SetActive(true);
        Sprite itemSprite = addedItemNotificationScript.CreateAddedItemNotification(newItem, count, newItemProperties.IsStackable); //Create "Added Item" Notification + store sprite

        if (!newItemDiscoveredScript.DiscoveredItems[item.name]) //If item has not yet been discovered:
        {
            newItemDiscoveredScript.gameObject.SetActive(true);
            newItemDiscoveredScript.CreateDiscoveredNotification(item.name, itemSprite);
        }

        return true;
    }

    public void DecreaseStackableItem(GameObject item)
    {
        ItemProperties itemsProperties = item.GetComponent<ItemProperties>();
        itemsProperties.ModifyCount(-1);

        if (itemsProperties.Count == 0)
        {
            lastItemWasRemoved = true;
            RemoveItem(item, itemsProperties);
            lastItemWasRemoved = false;
        }
    }

    public void ConsumeItem(GameObject item, ItemProperties itemsProperties)
    {
        if (interfaceCoroutinesScript.IsConsuming == true)
        {   
            Debug.Log("Currently Consuming");
            return;
        }

        interfaceCoroutinesScript.ConsumingItemCoroutine();

        itemsProperties.ModifyCount(-1);
        
        if (itemsProperties.Count == 0)
        {
            RemoveItem(item, itemsProperties);
        }

        if (itemsProperties.ItemType == ItemType.MeleePotion || itemsProperties.ItemType == ItemType.RangingPotion)
        {
            if (BuffListContainsItem(item.name)) //if item is in list, AKA buff is active, just renew buff
            {
                buffHandlerScript.RenewBuff(item);
            }

            else //otherwise, add buff
            {
                buffHandlerScript.AddBuff(item);
            }
        }

        //HEALTH/STAM SYSTEM STUFF HERE
    }

    public void AddBuffToList(GameObject item)
    {
        activeBuffsList.Add(item);
    }

    public void RemoveBuffFromList(GameObject item)
    {
        activeBuffsList.Remove(item);
    }

    bool BuffListContainsItem(string itemName)
    {
        for (int i = 0; i < activeBuffsList.Count; i++)
        {
            if (activeBuffsList[i].name == itemName)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveItem(GameObject item, ItemProperties itemsProperties)
    {
        int itemsIndex = itemsProperties.CurrentSlotIndex;

        if (itemsProperties.InterfaceType == InterfaceType.Keybind) //Removing a keybind item
        {
            ModifyKeybindArray(itemsIndex, false, item);
            //Necessary to not drop item as a pickup later
        }

        else if (itemsProperties.InterfaceType == InterfaceType.Equipped) //Removing an equipped item
        {
            ModifyEquippedArray(itemsIndex - 100, false, item);
            emptyEquippedIcons[(itemsIndex - 100)].SetActive(true); //set the type of equipment's empty slot icon active.
            itemsInInventoryList.Remove(item);
            UpdateKeybindActions(item, ActionType.Equip); //If keybound, sets keybind to equip so it can equip another item of that type
        }

        else if (itemsProperties.InterfaceType == InterfaceType.Inventory) //Removing an inventory item
        {
            ModifyInventoryArray(itemsIndex, false, item);
            itemsInInventoryList.Remove(item);
        }

        Destroy(item);
    }

    public void MoveToEmptyInventorySlot(int emptySlot, GameObject item)
    {
        item.transform.position = inventorySlots.transform.GetChild(emptySlot).position; //Change position to Slots position
        item.GetComponent<ClickHandler>().UpdateStartingPosition();
    }

    public void MoveToEmptyEquippedSlot(int emptySlot, GameObject item)
    {
        GameObject emptyEquippedIcon = emptyEquippedIcons[emptySlot];
        item.transform.position = emptyEquippedIcon.transform.position;
        emptyEquippedIcon.SetActive(false);
        item.GetComponent<ClickHandler>().UpdateStartingPosition();
    }

    public void ProcessMovingToEmptySlot(int index, GameObject item) //index of slot to move to + gameObject to move
    { //*** ADD 100 TO THE PASSED IN INDEX IF EQUIPPED ***
        ItemProperties itemsProperties = item.GetComponent<ItemProperties>();
        int previousIndex = itemsProperties.CurrentSlotIndex;
        int updatedIndex = index;

        if (previousIndex == updatedIndex) //If you drag and drop onto the same tile it was already on, return
        {
            if (itemsProperties.InterfaceType == InterfaceType.Inventory)
            {
                SetParentInsideInventory(item);
            }
            
            return;
        } 

        itemsProperties.ModifyCurrentSlotIndex(updatedIndex);

        if (updatedIndex < inventorySlotsCount) //--- moving to Inventory slot ---
        {
            itemsInInventoryArray[updatedIndex] = item;
            itemsProperties.SetInterfaceType(InterfaceType.Inventory);
            SetParentInsideInventory(item); //need parent set to InventoryItemParent
        }

        else //--- moving to Equipped slot ---
        {
            itemsInEquippedArray[(updatedIndex - 100)] = item;
            itemsProperties.SetInterfaceType(InterfaceType.Equipped);
            itemsProperties.SetActionType(ActionType.Unequip);

            if (itemsProperties.EquippedType != EquippedType.Quiver && itemsProperties.EquippedType != EquippedType.Bracelet) //If visible when equipped:
            {
                playerEquipmentScript.EquipPhysicalItem(item, updatedIndex - 100); //equip physical item
            }

            else if ((itemsProperties.ItemType == ItemType.Bolt) && (FindItemOfTypeAtEquippedIndex(3, ItemType.Crossbow) != null))
            { //If you equip a bolt while a crossbow is equipped:
                {
                    boltLauncherScript.LoadBolt();
                }
            }
        }

        if (previousIndex < inventorySlotsCount) //--- moving from Inventory slot ---
        {
            itemsInInventoryArray[previousIndex] = null;
        }

        else //--- moving from Equipped slot ---
        {
            itemsInEquippedArray[(previousIndex - 100)] = null;
            emptyEquippedIcons[(previousIndex - 100)].SetActive(true); //set the type of equipment's empty slot icon active.
            itemsProperties.SetActionType(ActionType.Equip);

            if (itemsProperties.EquippedType != EquippedType.Quiver && itemsProperties.EquippedType != EquippedType.Bracelet) //If visible when equipped:
            {
                playerEquipmentScript.UnequipPhysicalItem(previousIndex - 100); //unequip physical item
            }

            else if ((itemsProperties.ItemType == ItemType.Arrow) && (FindItemOfTypeAtEquippedIndex(3, ItemType.Bow) != null))
            {
                arrowLauncherScript.UnnockArrow();
            }

            else if ((itemsProperties.ItemType == ItemType.Bolt) && (FindItemOfTypeAtEquippedIndex(3, ItemType.Crossbow) != null))
            { //If you unequip a bolt while a crossbow is equipped:
                {
                    boltLauncherScript.UnloadBolt();
                }
            }
        }
    }

    public void SetParentInsideInventory(GameObject item)
    {
        item.transform.SetParent(inventoryItemParent);
    }

    public void SetParentOutsideInventory(GameObject item)
    {
        item.transform.SetParent(outsideItemParent);
    }

    public void ItemIsDragging(GameObject item)
    {
        isDragging = true;
        draggedItem = item;
    }

    public void ItemIsNotDragging()
    {
        isDragging = false;
        draggedItem = null;
    }

    public GameObject FindSameItemInInventory(GameObject item) //might have to update to compare names instead
    {
        for (int i = 0; i < itemsInInventoryArray.Length; i++)
        {
            if (itemsInInventoryArray[i] == null)
            {
                continue; //skips to next for loop
            }

            if (itemsInInventoryArray[i].name == item.name)
            {
                return itemsInInventoryArray[i];
            }
        }

        return null;
    }

    public GameObject FindSameItemInEquipped(GameObject item)
    {
        for (int i = 0; i < itemsInEquippedArray.Length; i++)
        {
            if (itemsInEquippedArray[i] == null)
            {
                continue; //skips to next for loop
            }

            if (itemsInEquippedArray[i].name == item.name)
            {
                return itemsInEquippedArray[i];
            }
        }

        return null;
    }

    public GameObject FindItemOfTypeInEquipped(EquippedType type)
    {
        int index = Array.IndexOf(itemTypeOfEachEquippedSlot, type); //Searches for specified object and returns the index of its first occurrence within an array

        if (index >= 0) //item of that type was found
        {
            return itemsInEquippedArray[index];
        }

        return null; //item of that type was not found
    }

    public GameObject FindItemOfTypeAtEquippedIndex(int index, ItemType itemType)
    {
        GameObject item = itemsInEquippedArray[index];

        if (item == null || item.GetComponent<ItemProperties>().ItemType != itemType)
        {
            return null;
        }

        else
        {
            return item;
        }
    }

    public int FindIndexOfEquippedSlot(EquippedType type)
    {
        int index = Array.IndexOf(itemTypeOfEachEquippedSlot, type); //Searches for specified object and returns the index of its first occurrence within an array
        return index;
    }

    public bool isInventoryFull()
    {
        for (int i = 0; i < itemsInInventoryArray.Length; i++)
        {
            if (itemsInInventoryArray[i] == null)
            {
                return false;
            }
        }

        return true;
    }

    public void SetEmptyEquippedIconsActive(int index, bool value)
    {
        emptyEquippedIcons[index].SetActive(value);
    }

    public void ModifyInventoryArray(int index, bool value, GameObject item)
    {
        if (value == false)
        {
            itemsInInventoryArray[index] = null;
            return;
        }

        else if (value == true)
        {
            itemsInInventoryArray[index] = item;
            return;
        }
    }

    public void ModifyEquippedArray(int index, bool value, GameObject item)
    {
        ItemProperties itemsProperties = item.GetComponent<ItemProperties>();
        EquippedType itemsEquippedType = itemsProperties.EquippedType;

        if (value == false)
        {
            itemsInEquippedArray[index] = null;

            if (itemsEquippedType != EquippedType.Quiver && itemsEquippedType != EquippedType.Bracelet) //If visible when equipped:
            {
                playerEquipmentScript.UnequipPhysicalItem(index); //unequip physical item
            }

            else if ((itemsProperties.ItemType == ItemType.Arrow) && (FindItemOfTypeAtEquippedIndex(3, ItemType.Bow) != null))
            {
                if (!lastItemWasRemoved)
                {
                    arrowLauncherScript.UnnockArrow();
                }
            }

            else if ((itemsProperties.ItemType == ItemType.Bolt) && (FindItemOfTypeAtEquippedIndex(3, ItemType.Crossbow) != null))
            { //If you unequip (or fire last) bolt while a crossbow is equipped:
                if (!lastItemWasRemoved) //Doesn't call UnloadBolt() if fired last bolt
                {
                    boltLauncherScript.UnloadBolt();
                }
            }

            return;
        }

        else if (value == true)
        {
            itemsInEquippedArray[index] = item;

            if (itemsEquippedType != EquippedType.Quiver && itemsEquippedType != EquippedType.Bracelet) //If visible when equipped:
            {
                playerEquipmentScript.UnequipPhysicalItem(index); //unequip previous physical item
                playerEquipmentScript.EquipPhysicalItem(item, index); //equip new physical item
            }

            else if ((itemsProperties.ItemType == ItemType.Arrow) && (FindItemOfTypeAtEquippedIndex(3, ItemType.Bow) != null))
            {
                arrowLauncherScript.UnnockArrow();
            }

            else if ((itemsProperties.EquippedType == EquippedType.Quiver) && (FindItemOfTypeAtEquippedIndex(3, ItemType.Crossbow) != null))
            {  //If you equip something to quiver while a crossbow is equipped:
                if (itemsProperties.ItemType == ItemType.Bolt)
                {
                    boltLauncherScript.UnloadBolt(); //unload previous bolt
                    boltLauncherScript.LoadBolt(); //load new bolt
                }

                else
                {
                    boltLauncherScript.UnloadBolt();
                }
            }

            return;
        }
    }

    public void ModifyKeybindArray(int index, bool value, GameObject item)
    {
        if (value == false)
        {
            itemsInKeybindArray[index] = null;
            return;
        }

        else if (value == true)
        {
            itemsInKeybindArray[index] = item;
            return;
        }
    }

    public GameObject CreateKeybind(GameObject draggedItem, int index, GameObject thisItem)
    {
        GameObject newItem = Instantiate(draggedItem, outsideItemParent); //Instantiate the item
        newItem.name = draggedItem.name; //So you can CheckForItem()
        newItem.transform.position = thisItem.transform.position; //Change position to Slots position
        newItem.GetComponent<ClickHandler>().UpdateStartingPosition();

        CanvasGroup newItemsCanvasGroup = newItem.GetComponent<CanvasGroup>(); //So you can interact with it and it looks right
        newItemsCanvasGroup.alpha = 1f;
        newItemsCanvasGroup.blocksRaycasts = true;

        ItemProperties newItemProperties = newItem.GetComponent<ItemProperties>();
        newItemProperties.SetInterfaceType(InterfaceType.Keybind);
        newItemProperties.ModifyCurrentSlotIndex(index);

        if (newItemProperties.IsStackable)
        {
            newItem.transform.Find("Text").GetComponent<TMP_Text>().enabled = false;
        }
        //maybe set stackable to false eventually

        return newItem;
    }

    public void UpdateKeybindActions(GameObject item, ActionType actionType)
    {
        for (int i = 0; i < itemsInKeybindArray.Length; i++)
        {
            if (itemsInKeybindArray[i] == null)
            {
                continue; //skips to next for loop
            }

            if (itemsInKeybindArray[i].name == item.name) //If equipped/unequipped item is keybound, update action type of each keybind
            {
                itemsInKeybindArray[i].GetComponent<ItemProperties>().SetActionType(actionType);
            }
        }
    }

    public GameObject ReturnKeybindItemOfIndex(int index)
    {
        return itemsInKeybindArray[index];
    }

    //Create a function just for KibbiumBracelet

    public void UpdateBraceletSlot(GameObject item)
    {
        GameObject emptyBraceletIcon = emptyEquippedIcons[8];

        if (emptyBraceletIcon.activeSelf)
        {
            item.transform.position = emptyBraceletIcon.transform.position;
            emptyBraceletIcon.SetActive(false);
            currentBracelet = item;
        }

        else //Only relevant once there are bracelet upgrades
        {
            item.transform.position = currentBracelet.transform.position;
            Destroy(currentBracelet);
            currentBracelet = item;
        }
    }

    public bool CreateBracelet(GameObject prefab) //returns true if created, false if not.
    {
        int firstEmptySlot = FindFirstEmptySlot(); //finds first empty slot

        startingBracelet.SetActive(true);
        startingBracelet.transform.position = inventorySlots.transform.GetChild(firstEmptySlot).position; //Change position to Slots position
        startingBracelet.GetComponent<ClickHandlerNewBracelet>().UpdateStartingPosition();

        addedItemNotificationScript.gameObject.SetActive(true);
        Sprite itemSprite = addedItemNotificationScript.CreateAddedItemNotification(startingBracelet, 1, false); //Create "Added Item" Notification + store sprite

        newItemDiscoveredScript.gameObject.SetActive(true);
        newItemDiscoveredScript.CreateDiscoveredNotification(startingBracelet.name, itemSprite);

        return true;
    }

    public GameObject FindBuffInList(string name)
    {
        for (int i = 0; i < activeBuffsList.Count; i++)
        {
            if (activeBuffsList[i].name == name)
            {
                return activeBuffsList[i];
            }
        }

        return null;
    }
}

//TO DO:

//Hover:
//Change effect if hovering over box - Box gets darker. Same with keybinds. CHECK RS
//

//Maybe Right click to open up box that says "Action, Drop"
//Maybe right click drops or clears

//Could show text in top left that shows action. Can be debug area too. For no action "No actions for" instead of "Consume" or "Equip"

//While dragging - highlight box youre hovered over like Runescape

//Popup saying when you add or increase inventory items

//Tab animation of inventory and gear going up
//Maybe Inventory/Equipped titles only show in tab mode. Animate them away/in on tabbing.

//Update look of UI buttons - not high priority

//Consumable functionality once health is implemented

//*If you have a full inventory but empty quiver slot, you currently can't fill quiver slot with an arrow pickup. May be intentional behavior, will have to see.

//Minor bug: Drag off inventory > Tab to disable it > Tab again while holding > Still has item selected
//Maybe fixed by adding something to OnDisable in DragHandler

//---UI Drag Script Locations ---
        //ItemSwapping/DragHandler - Prefabs
        //DragToDestroy - TabBackground
        //DragResetsPosition - Equipped>Slots, Keybinds>Slots, Inventory>Slots, Title>BackgroundBox, 
        //DragToEquipment - Equipped>Slots>Slot>Number
        //DragToKeybind - Keybinds>Slots>Slot>Number
        //DragToInventory - Inventory>Slots>Slot>Number

//Test: Script change for Github
