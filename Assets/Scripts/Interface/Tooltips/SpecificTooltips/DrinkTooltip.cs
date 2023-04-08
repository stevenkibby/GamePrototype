using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrinkTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI requirements;
    [SerializeField] TextMeshProUGUI stats;
    [SerializeField] ItemStats itemsStats;

    void Awake()
    {
        UpdateRequirementsText();
        UpdateStatsText();
    }

    void UpdateRequirementsText()
    {
        requirements.text = $"<color=#A9A9A9>(capped to the consumed drink tier)</color>";
    }

    void UpdateStatsText()
    {
        stats.text = $"<color=#FF2400>Tier {itemsStats.ConsumableTier} Drink</color>\nHealth Regeneration: <color=#FF2400>+{itemsStats.HealthRegen}</color>";
    }
}
