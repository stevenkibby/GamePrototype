using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Texture2D cursorDefault;
    [SerializeField] Texture2D cursorGreen;
    [SerializeField] Texture2D cursorRed;
    [SerializeField] ItemProperties itemsProperties;

    [SerializeField] GameObject tooltip;
    Transform outsideTooltipParent;

    void Awake()
    {
       tooltip.SetActive(false);
       outsideTooltipParent = GameObject.FindWithTag("OutsideTooltipParent").transform;
    }

    void OnEnable()
    {
        tooltip.transform.SetParent(gameObject.transform);
    }

    void OnDisable()
    {
        if (tooltip.activeSelf == true)
        {
            tooltip.SetActive(false);
        }

        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.transform.SetParent(outsideTooltipParent);
        tooltip.SetActive(true);
        UpdateCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
        tooltip.transform.SetParent(gameObject.transform);
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.ForceSoftware);
    }

    void UpdateCursor()
    {
        ActionType itemsActionType = itemsProperties.ActionType;

        if (itemsActionType == ActionType.NoAction)
        {
            Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.ForceSoftware);
            return;
        }

        if (itemsActionType == ActionType.Unequip)
        {
            Cursor.SetCursor(cursorRed, Vector2.zero, CursorMode.ForceSoftware);
            return;
        }

        //else it's equip or consume, turn green
        Cursor.SetCursor(cursorGreen, Vector2.zero, CursorMode.ForceSoftware);
    }
}
