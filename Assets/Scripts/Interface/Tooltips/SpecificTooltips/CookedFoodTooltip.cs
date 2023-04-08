using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CookedFoodTooltip : MonoBehaviour
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
        requirements.text = $"<color=#A9A9A9>(capped to the consumed food tier)</color>";
    }

    void UpdateStatsText()
    {
        stats.text = $"<color=#FDC534>Tier {itemsStats.ConsumableTier} Food</color>\nStamina Regeneration: <color=#FDC534>+{itemsStats.StaminaRegen}</color>";
    }
}
