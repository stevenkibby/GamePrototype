using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AxeTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI requirements;
    [SerializeField] TextMeshProUGUI stats;
    [SerializeField] ItemStats itemsStats;
    InventoryHandler inventoryHandlerScript;
    PlayerLevels playerLevelsScript;

    void Awake()
    {
        inventoryHandlerScript = GameObject.FindWithTag("Inventory").GetComponent<InventoryHandler>();
        playerLevelsScript = GameObject.FindWithTag("Player").GetComponent<PlayerLevels>(); //maybe use singleton for levels
    }

    void OnEnable()
    {
        UpdateEquipRequirement();
        UpdateStats();
    }

    void UpdateEquipRequirement()
    {
        if (playerLevelsScript.WoodcuttingLevel >= itemsStats.EquipRequirement) //If you have req, don't show it
        {
            requirements.text = null;
        }

        else //show it in red
        {
            requirements.text = $"<color=red>Requires Woodcutting Level: {itemsStats.EquipRequirement}</color>";
        }
    }

    void UpdateStats()
    {
        
        float itemsDamage = itemsStats.MaximumDamage;
        float equippedDamage = UpdateEquippedDamage();
        float potionsDamage = UpdatePotionsDamage();
        float totalDamage = itemsDamage + equippedDamage + potionsDamage;

        stats.text = $"<color=#80ffff>+{itemsDamage}</color> Woodcutting damage<size=14><color=#A9A9A9><size=10>\n\n</size>Max damage per chop: <color=#FFFFFF>{totalDamage}</color>\n( Item: <color=#FFFFFF>{itemsDamage}</color> | Equipped: <color=#FFFFFF>{equippedDamage}</color> | Potions: <color=#FFFFFF>{potionsDamage}</color> )</color></size>";
    }

    float UpdateEquippedDamage() //Currently: null | Potentially ring, cape, helm, chest, legs, offhand
    {
        return 0; //Gets equipped woodcutting items
    }

    float UpdatePotionsDamage()
    {
        return 0; //Gets current potion effects
    }
}
