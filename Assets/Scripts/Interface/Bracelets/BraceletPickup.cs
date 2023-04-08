using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraceletPickup : MonoBehaviour
{
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
            StartCoroutine(AttemptToCreateItem());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopAllCoroutines();
        }
    }

    IEnumerator AttemptToCreateItem()
    {
        while (true)
        {
            Debug.Log("Should call every frame?");
            if (inventoryHandlerScript.CreateBracelet(pickupItemPrefab) == true) //attempts to create nonstackable item. Only destroys pickup if created.
            {
                gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
}
