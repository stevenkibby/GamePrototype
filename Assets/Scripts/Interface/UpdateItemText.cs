using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateItemText : MonoBehaviour
{
    [SerializeField] ItemProperties itemPropertiesScript;
    [SerializeField] TMP_Text itemText;

    public void UpdateText()
    {
        itemText.text = itemPropertiesScript.Count.ToString();
    }
}
