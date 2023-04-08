using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThrownWeaponsTooltip : MonoBehaviour
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
        if (playerLevelsScript.ThrownWeaponLevel >= itemsStats.EquipRequirement) //If you have req, don't show it
        {
            requirements.text = null;
        }

        else //show it in red
        {
            requirements.text = $"<color=red>Requires Thrown Weapon Level: {itemsStats.EquipRequirement}</color>";
        }
    }

    void UpdateStats()
    {
        
        float itemsDamage = itemsStats.MaximumDamage;
        float equippedDamage = UpdateEquippedDamage();
        float potionsDamage = UpdatePotionsDamage();
        float totalDamage = itemsDamage + equippedDamage + potionsDamage;

        stats.text = $"<color=#80ffff>+{itemsDamage}</color> Thrown Weapon damage<size=14><color=#A9A9A9><size=10>\n\n</size>Max damage per hit: <color=#FFFFFF>{totalDamage}</color>\n( Item: <color=#FFFFFF>{itemsDamage}</color> | Equipped: <color=#FFFFFF>{equippedDamage}</color> | Potions: <color=#FFFFFF>{potionsDamage}</color> )</color></size>";
    }

    float UpdateEquippedDamage() //Currently: Helm, Chest, Legs | Potentially Offhand, Cape, Shield, Ring
    {
        float helmDamage;
        float chestDamage;
        float legsDamage;
        GameObject equippedHelm = inventoryHandlerScript.FindItemOfTypeAtEquippedIndex(1, ItemType.RangedArmor); //Get gameobject at index of equipped
        GameObject equippedChest = inventoryHandlerScript.FindItemOfTypeAtEquippedIndex(4, ItemType.RangedArmor); //Get gameobject at index of equipped
        GameObject equippedLegs = inventoryHandlerScript.FindItemOfTypeAtEquippedIndex(7, ItemType.RangedArmor); //Get gameobject at index of equipped

//Helm
        if (equippedHelm == null) //If nothing's equipped or equipped is wrong type, return 0
        {
            helmDamage = 0;
        }

        else
        {
            helmDamage = equippedHelm.GetComponent<ItemStats>().MaximumDamage;
        }
//Chest
        if (equippedChest == null) //If nothing's equipped or equipped is wrong type, return 0
        {
            chestDamage = 0;
        }

        else
        {
            chestDamage = equippedChest.GetComponent<ItemStats>().MaximumDamage;
        }
//Legs
        if (equippedLegs == null) //If nothing's equipped or equipped is wrong type, return 0
        {
            legsDamage = 0;
        }

        else
        {
            legsDamage = equippedLegs.GetComponent<ItemStats>().MaximumDamage;
        }

        return helmDamage + chestDamage + legsDamage;
    }

    float UpdatePotionsDamage()
    {
        return 0; //Gets current potion effects
    }
}
