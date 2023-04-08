using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ClickHandlerInertBracelet : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) //Triggers if left/middle/right button was clicked over this object
    {
        Debug.Log("It would be too dangerous to unequip your bracelet!");
    }
}
