using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArrowTooltip : MonoBehaviour
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
        stats.text = $"Bow Damage: <color=#80ffff>{itemsStats.MaximumDamage}</color>";
    }
}
