using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeleeArmorTooltip : MonoBehaviour
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
        requirements.text = null;
    }

    void UpdateStatsText()
    {
        stats.text = $"Melee Damage: <color=#80ffff>+{itemsStats.Modifier}%</color>\nMax Health <color=#FF2400>+{itemsStats.MaxHealth}</color>\nMax Stamina: <color=#FDC534>+{itemsStats.MaxStamina}</color>";
    }
}
