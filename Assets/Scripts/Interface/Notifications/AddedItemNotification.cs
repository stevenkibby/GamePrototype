using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddedItemNotification : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI spriteCount;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] GameObject currentSpriteImage;
    [SerializeField] Sprite[] spriteImages;

    string previousItemsName = "";
    Image currentSpriteTexture;
    float lifetime = 3f;
    bool isDisplaying = false;
    int previousCount = 0;

    void Awake()
    {
        currentSpriteTexture = currentSpriteImage.GetComponent<Image>();
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        isDisplaying = false;
        previousCount = 0;
    }

    public Sprite CreateAddedItemNotification(GameObject item, int count, bool isStackable)
    {
        StopAllCoroutines();
        StartCoroutine(SetLifetime());
        Sprite itemSprite = FindImageOfItem(item);
        currentSpriteTexture.sprite = itemSprite;

        if (isStackable)
        {
            if ((isDisplaying) && (previousItemsName == item.name)) //If this notification interupted previous one && previous notification was the same item:
            {
                spriteCount.text = (previousCount + count).ToString();
                previousCount += count;
            }

            else
            {
                spriteCount.text = count.ToString();
                previousCount = count;
            }
        }

        else //not stackable
        {
            spriteCount.text = null;
        }

        itemName.text = item.name;
        previousItemsName = item.name;
        return itemSprite;
    }

    Sprite FindImageOfItem(GameObject item)
    {
        for (int i = 0; i < spriteImages.Length; i++)
        {
            if (spriteImages[i].name == item.name)
            {
                return spriteImages[i];
            }
        }

        Debug.Log("No image found for item within spriteImages array.");
        return null;
    }

    IEnumerator SetLifetime()
    {
        isDisplaying = true;
        yield return new WaitForSecondsRealtime(lifetime);
        isDisplaying = false;
        previousCount = 0;
        gameObject.SetActive(false);
    }
}
