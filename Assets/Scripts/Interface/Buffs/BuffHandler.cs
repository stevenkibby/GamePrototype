using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffHandler : MonoBehaviour
{
    [SerializeField] Transform outsideItemParent;
    [SerializeField] InventoryHandler inventoryHandlerScript;
    [SerializeField] GameObject[] buffPrefabs;
    [SerializeField] GameObject[] buffSlots;
    
    GameObject[] itemsInBuffArray;
    int buffSlotsCount = 9;

    void Awake()
    {
        itemsInBuffArray = new GameObject[buffSlotsCount];
    }

    public void AddBuff(GameObject item)
    {
        int buffSlot = FindFirstEmptyBuffSlot();
        GameObject buffPrefab = FindBuffOfName(item.name);
        GameObject newBuff = Instantiate(buffPrefab, buffSlots[buffSlot].transform.position, Quaternion.identity, outsideItemParent);
        newBuff.name = item.name;
        itemsInBuffArray[buffSlot] = newBuff;
        inventoryHandlerScript.AddBuffToList(newBuff); //allows tooltips to tell if it's active

        TextMeshProUGUI newBuffsText = newBuff.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        ItemStats itemsStats = newBuff.GetComponent<ItemStats>();
        newBuffsText.text = ConvertToTime(itemsStats.SecondsTimer);

        newBuff.GetComponent<ControlBuffTimer>().StartControlBuffTimerCoroutine(itemsStats, newBuffsText);
    }

    public void RenewBuff(GameObject item) //Finds current buff and updates time remaining to new time
    {
        GameObject buff = itemsInBuffArray[FindSlotOfItem(item)];
        buff.GetComponent<ControlBuffTimer>().UpdateTimeRemaining();
    }

    int FindFirstEmptyBuffSlot()
    {
        for (int i = 0; i < itemsInBuffArray.Length; i++)
        {
            if (itemsInBuffArray[i] == null)
            {
                return i;
            }
        }

        Debug.Log("ERROR: Buff Array is full.");
        return itemsInBuffArray.Length + 1;
    }

    GameObject FindBuffOfName(string name)
    {
        for (int i = 0; i < buffPrefabs.Length; i++)
        {
            if (buffPrefabs[i].name == name)
            {
                return buffPrefabs[i];
            }
        }

        Debug.Log("ERROR: Name not found in buffprefabs list");
        return null;
    }

    public string ConvertToTime(int totalTime)
    {
        int minutes = (totalTime / 60);
        int seconds = (totalTime % 60);

        if (minutes == 0)
        {
            return $"{seconds}";
        }

        if (seconds < 10)
        {
            return  $"{minutes}:0{seconds}";
        }

        else
        {
            return $"{minutes}:{seconds}";
        }
    }

    void RearrangeBuffs(int buffSlot)
    {
        int currentSlot = buffSlot; //current slot = the slot that is now empty.

        while (true)
        {
            int nextBuffSlot = FindNextActiveBuffSlot(currentSlot); //Finds the next active buff above the now empty buff.

            if (nextBuffSlot == currentSlot) //If no buffs are active above current buff, return.
            {
                return;
            } 

            itemsInBuffArray[nextBuffSlot].transform.position = buffSlots[currentSlot].transform.position; //moves next buff to empty position.
            itemsInBuffArray[currentSlot] = itemsInBuffArray[nextBuffSlot]; //updates array by swapping nextBuffs with now empty one.
            itemsInBuffArray[nextBuffSlot] = null;
            currentSlot = nextBuffSlot; //sets currentSlot to the now empty one above.
        }
    }

    int FindNextActiveBuffSlot(int currentSlot)
    {
        for (int i = currentSlot + 1; i < itemsInBuffArray.Length; i++)
        {
            if (itemsInBuffArray[i] != null)
            {
                return i;
            }
        }

        return currentSlot;
    }

    int FindSlotOfItem(GameObject item) //Returns the slot number of the passed in item within the array
    {
        for (int i = 0; i < itemsInBuffArray.Length; i++)
        {
            if (itemsInBuffArray[i] == null)
            {
                continue;
            }

            if (itemsInBuffArray[i].name == item.name)
            {
                return i;
            }
        }

        Debug.Log("ERROR: Slot of item was not found.");
        return itemsInBuffArray.Length + 1;
    }

    public void FinishCoroutine(GameObject item)
    {
        int buffSlot = FindSlotOfItem(item);
        itemsInBuffArray[buffSlot] = null; //slot is updated via rearrangebuffs function from other items
        RearrangeBuffs(buffSlot);
        inventoryHandlerScript.RemoveBuffFromList(item);
        Destroy(item);
    }
}
