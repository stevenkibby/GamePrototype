using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Goes on Player > Player Capsule

//CalculateDamage()
//UpdateWeaponDamage
//UpdateModifier()
//RoundTotalDamage()

public class BowDamageCalculator : MonoBehaviour
{
    InventoryHandler inventoryHandlerScript;
    float updatedModifier;
    decimal roundedMaxDamage;

    void Awake()
    {
        inventoryHandlerScript = GameObject.FindWithTag("Inventory").GetComponent<InventoryHandler>();
    }

    //Not used atm, will be used in combat
    public decimal CalculateDamageHit(ItemStats itemsStats)
    {
        float baseDamage = UpdateBaseDamage(itemsStats);
        float modifier = UpdateModifier();
        float exactMaxDamage = UpdateMaxDamage(baseDamage, modifier);
        //Calculate random number lower than max damage
        //Then round that number instead.
        decimal roundedDamage = RoundDamage(exactMaxDamage);
        return roundedDamage;

        //MORE once you use bell curve to find random number and consider level to adjust the curve! No longer return RoundTotalDamage.
        //ALSO for calculating damage - You'll want to roll hit using only weapon damage. THEN add modifier to that hit for actual damage.
        //Otherwise the modifier does much less than stated.
    }

    public float UpdateBaseDamage(ItemStats itemsStats)
    {
        float bowDamage = itemsStats.MaximumDamage;
        float arrowDamage;
        float baseDamage;

        GameObject equippedArrows = inventoryHandlerScript.FindItemOfTypeAtEquippedIndex(2, ItemType.Arrow); //Get gameobject at index of equipped

        if (equippedArrows == null) //If nothing's equipped or equipped is wrong type, return 0
        {
            arrowDamage = 0;
        }

        else
        {
            arrowDamage = equippedArrows.GetComponent<ItemStats>().MaximumDamage;
        }

        baseDamage = bowDamage + arrowDamage;
        return baseDamage;
    }

    public float UpdateModifier() //Currently: Helm, Chest, Legs, Ring, Ranging Potion | Potentially Offhand, Cape, Shield
    {
        float helmModifier;
        float chestModifier;
        float legsModifier;
        float ringModifier;
        float potionModifier;
        float totalModifier;

        GameObject equippedHelm = inventoryHandlerScript.FindItemOfTypeAtEquippedIndex(1, ItemType.RangedArmor); //Get gameobject at index of equipped
        GameObject equippedChest = inventoryHandlerScript.FindItemOfTypeAtEquippedIndex(4, ItemType.RangedArmor); //Get gameobject at index of equipped
        GameObject equippedLegs = inventoryHandlerScript.FindItemOfTypeAtEquippedIndex(7, ItemType.RangedArmor); //Get gameobject at index of equipped
        GameObject equippedRing = inventoryHandlerScript.FindItemOfTypeAtEquippedIndex(6, ItemType.RangedRing); //Get gameobject at index of equipped
        GameObject activePotion = inventoryHandlerScript.FindBuffInList("Ranging Potion");

        //Helm
        if (equippedHelm == null) //If nothing's equipped or equipped is wrong type, return 0
        {
            helmModifier = 0;
        }

        else
        {
            helmModifier = equippedHelm.GetComponent<ItemStats>().Modifier;
        }

        //Chest
        if (equippedChest == null) //If nothing's equipped or equipped is wrong type, return 0
        {
            chestModifier = 0;
        }

        else
        {
            chestModifier = equippedChest.GetComponent<ItemStats>().Modifier;
        }

        //Legs
        if (equippedLegs == null) //If nothing's equipped or equipped is wrong type, return 0
        {
            legsModifier = 0;
        }

        else
        {
            legsModifier = equippedLegs.GetComponent<ItemStats>().Modifier;
        }

        //Ring
        if (equippedRing == null) //If nothing's equipped or equipped is wrong type, return 0
        {
            ringModifier = 0;
        }

        else
        {
            ringModifier = equippedRing.GetComponent<ItemStats>().Modifier;
        }

        //Potion

        if (activePotion == null) //If no active potion of that type, return 0
        {
            potionModifier = 0;
        }

        else
        {
            potionModifier = activePotion.GetComponent<ItemStats>().Modifier;
        }

        totalModifier = helmModifier + chestModifier + legsModifier + ringModifier + potionModifier;
        return totalModifier;
    }

    public float UpdateMaxDamage(float baseDamage, float modifier)
    {
        float exactMaxDamage = (baseDamage + (baseDamage * (modifier * 0.01f)));
        return exactMaxDamage;
    }

    public decimal RoundDamage(float exactDamage)
    {
        decimal roundedDamage = decimal.Round(((decimal)exactDamage), 1); //googled "convert float to decimal" and "rounding to one decimal point with decimal.Round"
        return roundedDamage;
    }

}
