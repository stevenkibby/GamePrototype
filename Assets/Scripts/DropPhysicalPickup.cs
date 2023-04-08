using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPhysicalPickup : MonoBehaviour
{
    [SerializeField] GameObject[] droppableItems;
    [SerializeField] Transform pickupsParent;
    Vector3 pickupSpawnPoint = new Vector3(0f, 0f, 1f);
    Vector3 pickupDropDirection = new Vector3(0f, 0f, 1f);
    float dropForce = 400f;

    public void DropPickup(GameObject spriteItem, int pickupAmount)
    {
        GameObject pickupPrefab = FindPickupOfSprite(spriteItem);
        GameObject pickup = Instantiate(pickupPrefab, (transform.position + (transform.forward * 2)), Quaternion.identity, pickupsParent);
        pickup.GetComponent<Pickup>().SetPickupAmount(pickupAmount);
        pickup.GetComponent<Rigidbody>().AddRelativeForce((transform.forward + transform.up) * dropForce);
    }

    GameObject FindPickupOfSprite(GameObject spriteItem)
    {
        for (int i = 0; i < droppableItems.Length; i++)
        {
            if (droppableItems[i].name == spriteItem.name)
            {
                return droppableItems[i];
            }
        }

        Debug.Log("Droppable item not found");
        return null;
    }
}
