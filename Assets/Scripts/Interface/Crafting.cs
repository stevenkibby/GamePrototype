using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] GameObject[] attachedImages;

    public void SelectItem()
    {
        attachedImages[0].transform.position = transform.position;
        attachedImages[0].SetActive(true);
    }
}
