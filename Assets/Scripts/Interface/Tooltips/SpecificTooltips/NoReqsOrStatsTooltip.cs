using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoReqsOrStatsTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI requirements;
    [SerializeField] TextMeshProUGUI stats;

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
        stats.text = $"No stats";
    }
}
