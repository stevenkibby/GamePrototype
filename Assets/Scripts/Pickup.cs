using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    [SerializeField] int pickupAmount = 1;
    [SerializeField] GameObject pickupItemPrefab;
    [SerializeField] bool isStackable;

    InventoryHandler inventoryHandlerScript;
    Rigidbody pickupsRigidbody;
    Collider pickupsCollider;

    void Awake()
    {
        inventoryHandlerScript = FindObjectOfType<InventoryHandler>();
        pickupsRigidbody = GetComponent<Rigidbody>();
        pickupsCollider = GetComponent<Collider>();
        //Possibly start coroutine to destroy object after x time
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            pickupsRigidbody.isKinematic = true;
            pickupsCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isStackable)
            {
                StartCoroutine(AttemptToIncreaseStackableItem());
            }

            else
            {
                StartCoroutine(AttemptToCreateItem());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopAllCoroutines();
        }
    }

    IEnumerator AttemptToIncreaseStackableItem()
    {
        while (true)
        {
            if (inventoryHandlerScript.IncreaseStackableItem(pickupItemPrefab, pickupAmount) == true) //attempts to create stackable item. Only destroys pickup if created.
            {
                gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator AttemptToCreateItem()
    {
        while (true)
        {
            if (inventoryHandlerScript.CreateItem(pickupItemPrefab, pickupAmount) == true) //attempts to create nonstackable item. Only destroys pickup if created.
            {
                gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }

    public void SetPickupAmount(int amount)
    {
        pickupAmount = amount;
    }
}
