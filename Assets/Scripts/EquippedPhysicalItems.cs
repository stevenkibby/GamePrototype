using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedPhysicalItems : MonoBehaviour
{   
    [SerializeField] InventoryHandler inventoryHandlerScript;
    [SerializeField] GameObject[] allPhysicalItems;

    GameObject[] equippedPhysicalItems;
    int equippedSlotsCount = 9;

    void Awake()
    {
        equippedPhysicalItems = new GameObject[equippedSlotsCount];
    }

    public void EquipPhysicalItem(GameObject itemSprite, int index)
    {
        for (int i = 0; i < allPhysicalItems.Length; i++)
        {
            if (allPhysicalItems[i].name == itemSprite.name)
            {
                equippedPhysicalItems[index] = allPhysicalItems[i];
                equippedPhysicalItems[index].SetActive(true);
                return;
            }
        }

        Debug.Log("Item was not found on player");
    }

    public void UnequipPhysicalItem(int index)
    {
        GameObject equippedItem = equippedPhysicalItems[index];

        if (equippedItem == null) { return; }

        equippedItem.SetActive(false);
        equippedPhysicalItems[index] = null;
    }
}
