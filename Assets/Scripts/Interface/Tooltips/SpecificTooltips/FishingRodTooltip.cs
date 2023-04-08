using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishingRodTooltip : MonoBehaviour
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
        if (playerLevelsScript.FishingLevel >= itemsStats.EquipRequirement) //If you have req, don't show it
        {
            requirements.text = null;
        }

        else //show it in red
        {
            requirements.text = $"<color=red>Requires Fishing Level: {itemsStats.EquipRequirement}</color>";
        }
    }

    void UpdateStats()
    {
        
        float itemsChance = itemsStats.MaximumDamage;
        float equippedChance = UpdateEquippedDamage();
        float potionsChance = UpdatePotionsDamage();
        float totalChance = itemsChance + equippedChance + potionsChance;

        stats.text = $"<color=#80ffff>+{itemsChance}</color> Catch Chance<size=14><color=#A9A9A9><size=10>\n\n</size>Max catch chance per bait: <color=#FFFFFF>{totalChance}%</color>\n( Item: <color=#FFFFFF>{itemsChance}</color> | Equipped: <color=#FFFFFF>{equippedChance}</color> | Potions: <color=#FFFFFF>{potionsChance}</color> )</color></size>";
    }

    float UpdateEquippedDamage() //Currently: null | Potentially ring, cape, helm, chest, legs, offhand
    {
        return 0; //Gets equipped fishing items
    }

    float UpdatePotionsDamage()
    {
        return 0; //Gets current potion effects
    }
}
