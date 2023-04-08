using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragToBraceletSlot : MonoBehaviour, IDropHandler
{
    
    //CodeMonkey Drag/Drop video: https://www.youtube.com/watch?v=BGr-7GZJNXg

    [SerializeField] ClickHandlerNewBracelet startingBraceletScript;
    [SerializeField] GameObject braceletBackground;
    [SerializeField] GameObject braceletDragSlot;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        GameObject draggedItem = eventData.pointerDrag;
        if (draggedItem == null) { return; }
        
        startingBraceletScript.EquipBracelet();
        Destroy(braceletDragSlot);
        Destroy(braceletBackground);

    }
}

