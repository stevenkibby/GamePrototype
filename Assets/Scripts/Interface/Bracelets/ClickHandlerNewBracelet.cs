using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ClickHandlerNewBracelet : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    //CodeMonkey Drag/Drop video: https://www.youtube.com/watch?v=BGr-7GZJNXg

    [SerializeField] GameObject inertKibbiumBraceletPrefab;
    [SerializeField] Canvas tabCanvas;
    [SerializeField] InventoryHandler inventoryHandlerScript;
    [SerializeField] GameObject outsideItemParent;
    [SerializeField] GameObject braceletBackground;
    [SerializeField] GameObject braceletDragSlot;

    GameObject braceletSlot;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    GameObject temporaryClone;
    
    Vector2 startingPosition;
    public Vector2 StartingPosition { get { return startingPosition; } }

    bool hasBeenDragged = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData eventData) //Triggers if left/middle/right button was clicked over this object
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        
        UpdateStartingPosition();
        canvasGroup.alpha = .5f; //The permanent transparency of the clone
        canvasGroup.blocksRaycasts = false; //raycast goes through this object and lands on object below it like item slot
        inventoryHandlerScript.SetParentOutsideInventory(gameObject); //Sets it's parent to OtherItemParent
        transform.SetAsLastSibling(); //so dragged image always appears above inventory images
        
        if (temporaryClone == null)
        {
            temporaryClone = Instantiate(gameObject, gameObject.transform.parent);
            Color temporaryClonesColor = new Color(0f, 0f, 0f, 1f);
            temporaryClone.transform.Find("Image").GetComponent<Image>().color = temporaryClonesColor; //Set color of temporaryClone to look like a shadow
            Transform temporaryClonesText = temporaryClone.transform.Find("Text");

            if (temporaryClonesText != null)
            {
                temporaryClonesText.GetComponent<TMP_Text>().enabled = false; //Disable clone's text if parent has text enabled
            }
        }

        else //temporaryClone exists but was disabled
        {
            temporaryClone.SetActive(true);
        }

        temporaryClone.transform.position = transform.position;
        temporaryClone.transform.SetAsFirstSibling();
        canvasGroup.alpha = .5f; //The transparency of the dragged item
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        canvasGroup.alpha = 1f; //returns to no transparency on release
        canvasGroup.blocksRaycasts = true; //Can be interacted with again
        temporaryClone.SetActive(false);

        if (!hasBeenDragged) //------- Releasing left mouse without a drag - AKA clicking - Action or nothing/small animation if no action -------
        {
            EquipBracelet();
        }
    }

    public void EquipBracelet()
    {
        GameObject inertKibbiumBracelet = Instantiate(inertKibbiumBraceletPrefab, outsideItemParent.transform); //Instantiate the item
        inventoryHandlerScript.SetParentOutsideInventory(inertKibbiumBracelet); //Sets parent to equipped
        inertKibbiumBracelet.name = gameObject.name; //Updates name for tooltip
        inventoryHandlerScript.UpdateBraceletSlot(inertKibbiumBracelet);
        Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData evenData)
    {
        hasBeenDragged = true;
        inventoryHandlerScript.ItemIsDragging(gameObject);
        braceletBackground.SetActive(true);
        braceletDragSlot.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        rectTransform.anchoredPosition += eventData.delta / tabCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        UpdateStartingPosition();
        canvasGroup.alpha = 1f; //returns to no transparency on release
        canvasGroup.blocksRaycasts = true; //Can be interacted with again
        hasBeenDragged = false;
        inventoryHandlerScript.ItemIsNotDragging();
        temporaryClone.SetActive(false);

        braceletBackground.SetActive(false);
        braceletDragSlot.SetActive(false);
    }

    public void UpdateStartingPosition()
    {
        startingPosition = transform.position;
    }
    
    void OnDestroy() //Destroy temporaryClone if this object is also destroyed
    {
        if (temporaryClone != null)
        {
            Destroy(temporaryClone);
        }
    }

    public void SetParentInsideInventory()
    {
        inventoryHandlerScript.SetParentInsideInventory(gameObject);
    }

    public void DisableCurrentDrag()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        transform.position = startingPosition;
        temporaryClone.SetActive(false);
    }
}
