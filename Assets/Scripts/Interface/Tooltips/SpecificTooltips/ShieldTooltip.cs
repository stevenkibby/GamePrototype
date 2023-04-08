using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShieldTooltip : MonoBehaviour
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
        stats.text = $"Negates <color=#80ffff>{itemsStats.MaximumDamage}</color> damage per hit<size=14><color=#A9A9A9><size=10>\n\n</size>Block: consumes negated value in stamina\nParry: restores negated value in stamina</color></size>";
    }
}
