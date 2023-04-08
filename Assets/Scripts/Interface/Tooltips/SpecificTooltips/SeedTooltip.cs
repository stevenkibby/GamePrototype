using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI requirements;
    [SerializeField] TextMeshProUGUI stats;
    [SerializeField] ItemStats itemsStats;
    PlayerLevels playerLevelsScript;

    void Awake()
    {
        playerLevelsScript = GameObject.FindWithTag("Player").GetComponent<PlayerLevels>(); //maybe use singleton for levels
        UpdateCookRequirement();
        UpdateStats();
    }

    void UpdateCookRequirement()
    {
        if (playerLevelsScript.CookingLevel >= itemsStats.UseRequirement) //If you have req, don't show it
        {
            requirements.text = null;
        }

        else //show it in red
        {
            requirements.text = $"<color=red>Requires Farming Level: {itemsStats.UseRequirement}</color>";
        }
    }

    void UpdateStats()
    {
        stats.text = $"<color=#FDC534>Tier {itemsStats.ConsumableTier} Food";
    }
}
