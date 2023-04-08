using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlBuffTimer : MonoBehaviour
{
    BuffHandler buffHandlerScript;
    int timeRemaining;
    TextMeshProUGUI buffsText;

    void Awake()
    {
        buffHandlerScript = GameObject.FindWithTag("BuffHandler").GetComponent<BuffHandler>();
        buffsText = gameObject.transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void StartControlBuffTimerCoroutine(ItemStats itemsStats, TextMeshProUGUI itemsText)
    {
        StartCoroutine(ControlBuffTimerCoroutine(itemsStats, itemsText));
    }

    IEnumerator ControlBuffTimerCoroutine(ItemStats itemsStats, TextMeshProUGUI itemsText)
    {
        timeRemaining = itemsStats.SecondsTimer;

        while (timeRemaining > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            timeRemaining -= 1;
            itemsText.text = buffHandlerScript.ConvertToTime(timeRemaining);
        }

        buffHandlerScript.FinishCoroutine(gameObject);
    }

    public void UpdateTimeRemaining()
    {
        timeRemaining = GetComponent<ItemStats>().SecondsTimer;
        buffsText.text = buffHandlerScript.ConvertToTime(timeRemaining);
    }
}
