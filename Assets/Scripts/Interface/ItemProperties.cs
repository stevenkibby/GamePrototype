using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProperties : MonoBehaviour
{
    [SerializeField] int count = 0;
    public int Count { get { return count; } }

    [SerializeField] bool isStackable;
    public bool IsStackable { get { return isStackable; } }

    [SerializeField] ItemType itemType;
    public ItemType ItemType { get { return itemType; } }

    [SerializeField] EquippedType equippedType;
    public EquippedType EquippedType { get { return equippedType; } }

    [SerializeField] ActionType actionType;
    public ActionType ActionType { get { return actionType; } }

    [SerializeField] InterfaceType interfaceType;
    public InterfaceType InterfaceType { get { return interfaceType; } }

    [SerializeField] string description;
    public string Description { get { return description; } }

    [SerializeField] UpdateItemText updateItemTextScript;

    int currentSlotIndex = 0;
    public int CurrentSlotIndex { get { return currentSlotIndex; } }

    public void ModifyCount(int value)
    {
        count += value;

        if (isStackable)
        {
            updateItemTextScript.UpdateText();
        }
    }

    public void ModifyCurrentSlotIndex(int value)
    {
        currentSlotIndex = value;
    }

    public void SetActionType(ActionType type)
    {
        actionType = type;
    }

    public void SetInterfaceType(InterfaceType type)
    {
        interfaceType = type;
    }
}
