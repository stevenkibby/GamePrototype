using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersatileArmorTooltip : MonoBehaviour
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
        stats.text = $"Max Health: <color=#FF2400>+{itemsStats.MaxHealth}</color>\nMax Stamina: <color=#FDC534>+{itemsStats.MaxStamina}</color>";
    }
}
