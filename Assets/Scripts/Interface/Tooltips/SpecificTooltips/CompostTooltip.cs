using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompostTooltip : MonoBehaviour
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

    void UpdateRequirementsText()
    {
        if (playerLevelsScript.FarmingLevel >= itemsStats.EquipRequirement) //If you have req, don't show it
        {
            requirements.text = null;
        }

        else //show it in red
        {
            requirements.text = $"<color=red>Requires Farming Level: {itemsStats.EquipRequirement}</color>";
        }
    }

    void UpdateStatsText()
    {
        stats.text = $"Max yield per seed: <color=#80ffff>{itemsStats.MaximumDamage}</color>";
    }
}
