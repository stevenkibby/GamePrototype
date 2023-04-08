using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickaxeTooltip : MonoBehaviour
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
        if (playerLevelsScript.MiningLevel >= itemsStats.EquipRequirement) //If you have req, don't show it
        {
            requirements.text = null;
        }

        else //show it in red
        {
            requirements.text = $"<color=red>Requires Mining Level: {itemsStats.EquipRequirement}</color>";
        }
    }

    void UpdateStats()
    {
        
        float pickaxeDamage = itemsStats.MaximumDamage;
        float equippedDamage = UpdateEquippedDamage();
        float potionsDamage = UpdatePotionsDamage();
        float totalDamage = pickaxeDamage + equippedDamage + potionsDamage;

        stats.text = $"<color=#80ffff>+{pickaxeDamage}</color> Mining damage<size=14><color=#A9A9A9><size=10>\n\n</size>Max damage per swing: <color=#FFFFFF>{totalDamage}</color>\n( Item: <color=#FFFFFF>{pickaxeDamage}</color> | Equipped: <color=#FFFFFF>{equippedDamage}</color> | Potions: <color=#FFFFFF>{potionsDamage}</color> )</color></size>";
    }

    float UpdateEquippedDamage() //Current: null | Potentially: Mining helmet w/ light, ring, cape, chest, legs, offhand
    {
        return 0; //Gets equipped mining items
    }

    float UpdatePotionsDamage()
    {
        return 0; //Gets current potion effects
    }
}
