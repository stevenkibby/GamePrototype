using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaitTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI requirements;
    [SerializeField] TextMeshProUGUI stats;
    [SerializeField] ItemStats itemsStats;
    PlayerLevels playerLevelsScript;

    void Awake()
    {
        playerLevelsScript = GameObject.FindWithTag("Player").GetComponent<PlayerLevels>(); //maybe use singleton for levels
        UpdateStatsText();
    }

    void OnEnable()
    {
        UpdateRequirementsText();
    }

    void UpdateStatsText()
    {
        stats.text = $"No stats";
    }

    void UpdateRequirementsText()
    {
        if (playerLevelsScript.FishingLevel >= itemsStats.UseRequirement) //If you have req, don't show it
        {
            requirements.text = null;
        }

        else //show it in red
        {
            requirements.text = $"<color=red>Requires Fishing Level: {itemsStats.UseRequirement}</color>";
        }
        //In future, maybe say: Tier 1 Bait, etc. to be able to catch that tier of fish.
    }
}
