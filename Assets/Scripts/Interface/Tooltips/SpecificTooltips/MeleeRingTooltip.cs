using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeleeRingTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI requirements;
    [SerializeField] TextMeshProUGUI stats;
    [SerializeField] ItemStats itemsStats;

    void Awake()
    {
        UpdateRequirements();
        UpdateStats();
    }

    void UpdateRequirements()
    {
        requirements.text = null;
    }

    void UpdateStats()
    {
        stats.text = $"<color=#80ffff>+{itemsStats.Modifier}%</color> Melee Damage";
    }
}
