using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RangedArmorTooltip : MonoBehaviour
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
        stats.text = $"<color=#80ffff>+{itemsStats.Modifier}%</color> Ranged Damage\n<color=#FF2400>+{itemsStats.MaxHealth}</color> Max Health\n<color=#FDC534>+{itemsStats.MaxStamina}</color> Max Stamina";
    }
}
