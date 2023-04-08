using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceCoroutines : MonoBehaviour
{
    float consumeDelay = 1f;
    bool isConsuming;
    public bool IsConsuming { get { return isConsuming; } }


    public void ConsumingItemCoroutine()
    {
        StartCoroutine(ConsumingItem());
    }

    IEnumerator ConsumingItem()
    {
        isConsuming = true;
        yield return new WaitForSeconds(consumeDelay);
        isConsuming = false;
    }
}
