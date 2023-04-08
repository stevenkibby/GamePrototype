using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : MonoBehaviour
{
    //Health variables
    float defaultHealth = 100f;
    float maxHealth = 100f;
    float currentHealth;
    float percentHealth;

    //Vignette variables
    [SerializeField] PostProcessVolume volume;
    float maxOnHitIntensity = 0.5f;
    float maxStaticIntensity = 0.5f;
    float upTime = 0.05f;
    float downTime = 1f;
    float startIntensity;
    float newIntensity;
    PlayerDeath deathScript;
    Vignette vignette;

    //RedFade variables
    [SerializeField] Image redImage;
    float currentOpacity = 0f;
    float maxOpacity = 0.05f;
    float startOpacity;

    void Awake()
    {
        deathScript = GetComponent<PlayerDeath>();
        volume.profile.TryGetSettings(out vignette); //googled way to get Vignette settings
    }

    void OnEnable()
    {
        currentHealth = defaultHealth;
        redImage.color = new Color (1, 1, 1, 0);
        redImage.enabled = false;
        UpdateStaticVignette();
    }

    public void TakeDamage(float value)
    {   
        currentHealth -= value;
        StopAllCoroutines();
        StartCoroutine(ProcessVignette());

        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            deathScript.KillPlayer();
        }
    }

    void UpdateStaticVignette()
    {
        float percentHealth = currentHealth / maxHealth;
        vignette.intensity.value = Mathf.Lerp(maxStaticIntensity, 0f, percentHealth);
    }

    IEnumerator ProcessVignette()
    {
        startIntensity = vignette.intensity.value;
        newIntensity = Mathf.Lerp(maxStaticIntensity, 0f, currentHealth / maxHealth);

        startOpacity = currentOpacity;
        redImage.enabled = true;

        for (float currentTime = 0; currentTime < upTime; currentTime += Time.deltaTime)
        {
            float timePercent = currentTime / upTime;
            vignette.intensity.value = Mathf.Lerp(startIntensity, maxOnHitIntensity, timePercent);

            currentOpacity = Mathf.Lerp(startOpacity, maxOpacity, timePercent);
            redImage.color = new Color (1f, 1f, 1f, currentOpacity);

            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.05f);
        startIntensity = vignette.intensity.value;
        startOpacity = currentOpacity;

        for (float currentTime = 0; currentTime < downTime; currentTime += Time.deltaTime)
        {
            float timePercent = currentTime / downTime;
            vignette.intensity.value = Mathf.Lerp(startIntensity, newIntensity, currentTime / downTime);

            currentOpacity = Mathf.Lerp(maxOpacity, 0f, timePercent);
            redImage.color = new Color (1f, 1f, 1f, currentOpacity);

            yield return null;
        }

        redImage.enabled = false;
    }


}

