
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BowTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI requirements;
    [SerializeField] TextMeshProUGUI stats;
    [SerializeField] ItemStats itemsStats;
    GameObject player;
    PlayerLevels playerLevelsScript;
    BowDamageCalculator bowDamageCalculatorScript;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerLevelsScript = player.GetComponent<PlayerLevels>(); //maybe use singleton for levels
        bowDamageCalculatorScript = player.transform.Find("DamageCalculators").GetComponent<BowDamageCalculator>();
    }

    void OnEnable()
    {
        UpdateRequirementsText();
        UpdateStatsText();
    }

    void UpdateRequirementsText()
    {
        if (playerLevelsScript.BowLevel >= itemsStats.EquipRequirement) //If you have req, don't show it
        {
            requirements.text = null;
        }

        else //show it in red
        {
            requirements.text = $"<color=red>Requires Bow Level: {itemsStats.EquipRequirement}</color>";
        }
    }

    void UpdateStatsText()
    {
        float bowDamage = itemsStats.MaximumDamage;
        float baseDamage = bowDamageCalculatorScript.UpdateBaseDamage(itemsStats); //Weapon damage (weapon + possible ammo)
        float modifier = bowDamageCalculatorScript.UpdateModifier(); //Modifier (all applicable modifiers)
        float exactMaxDamage = bowDamageCalculatorScript.UpdateMaxDamage(baseDamage, modifier); //Max damage per hit
        decimal roundedMaxDamage = bowDamageCalculatorScript.RoundDamage(exactMaxDamage);
        stats.text = $"Bow Damage: <color=#80ffff>{bowDamage}</color><size=14><color=#A9A9A9><size=10>\n\n</size>Max damage per hit: <color=#FFFFFF>{roundedMaxDamage}</color>\n( Weapon: <color=#FFFFFF>{baseDamage}</color> | Modifier: <color=#FFFFFF>+{modifier}%</color> )</color></size>";
    }
}
