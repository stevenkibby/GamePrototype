using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{

    //https://www.youtube.com/watch?v=y2N_J391ptg (Simple Tooltip tutorial)

    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI action;
    [SerializeField] ItemProperties itemsProperties;
    [SerializeField] Transform spritePrefabParent;

    Vector3 mouseAdjust;

    void Awake()
    {
        UpdateDescription();
    }

    void OnEnable()
    {
        UpdateMouseAdjust();
        UpdateTooltipPosition();
        UpdateTitle();
        UpdateAction();
    }

    void Update()
    {
        UpdateTooltipPosition();
    }

    void UpdateDescription()
    {
        description.text = itemsProperties.Description;
    }

    void UpdateMouseAdjust()
    {
        InterfaceType itemsInterfaceType = itemsProperties.InterfaceType;
        
        if (itemsInterfaceType == InterfaceType.Equipped || itemsInterfaceType == InterfaceType.Buff)
        {
            mouseAdjust = new Vector3(-180f * transform.lossyScale.x, 130f * transform.lossyScale.y, 0f); //150 and 100 are defaults on mouse cursor
        }

        else
        {
            mouseAdjust = new Vector3(180f * transform.lossyScale.x, 130f * transform.lossyScale.y, 0f);
        }
    }

    void UpdateTooltipPosition()
    {
        transform.position = Input.mousePosition + mouseAdjust;
    }

    void UpdateTitle()
    {
        title.text = $"{itemsProperties.name}";
    }

    void UpdateAction()
    {
        ActionType itemsActionType = itemsProperties.ActionType;

        if (itemsActionType == ActionType.NoAction)
        {
            action.text = "<color=black>(No Action)</color>";
        }

        else
        {
            action.text = $"{itemsProperties.ActionType}:";
        }
    }
}

