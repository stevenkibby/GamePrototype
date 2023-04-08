using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewItemDiscovered : MonoBehaviour
{
    Dictionary<string, bool> discoveredItems = new Dictionary<string, bool>()
    {
        {"Amethyst Ring", false},
        {"Amethyst", false},
        {"Berries", false},
        {"Berry Seeds", false},
        {"Boiled Water", false},
        {"Bone Dagger", false},
        {"Bone Pickaxe", false},
        {"Bone", false},
        {"Bronze Arrow", false},
        {"Bronze Bar", false},
        {"Bronze Bolt", false},
        {"Bronze Dart", false},
        {"Bronze Helmet", false},
        {"Bronze Mace", false},
        {"Bronze Platebody", false},
        {"Bronze Platelegs", false},
        {"Bronze Shield", false},
        {"Bronze Spear", false},
        {"Bronze Sword", false},
        {"Burl", false},
        {"Cloth Cape", false},
        {"Cloth Hood", false},
        {"Cloth Pants", false},
        {"Cloth Tunic", false},
        {"Cloth", false},
        {"Compost", false},
        {"Cooked Meat", false},
        {"Cooked Salmon", false},
        {"Copper Ore", false},
        {"Corn Kernels", false},
        {"Corn", false},
        {"Earth Worm", false},
        {"Feather", false},
        {"Flax", false},
        {"Grass", false},
        {"Hide Body", false},
        {"Hide Hat", false},
        {"Hide Legs", false},
        {"Hide", false},
        {"Kibbium Bracelet", false},
        {"Melee Potion", false},
        {"Pond Water", false},
        {"Quartz Ring", false},
        {"Quartz", false},
        {"Rain Water", false},
        {"Ranging Potion", false},
        {"Raw Meat", false},
        {"Raw Salmon", false},
        {"Silver Bar", false},
        {"Silver Ore", false},
        {"Silver Ring", false},
        {"Sinew", false},
        {"Stone Axe", false},
        {"Stone Battleaxe", false},
        {"Stone", false},
        {"Tin Ore", false},
        {"Twine", false},
        {"Wood", false},
        {"Wooden Arrow", false},
        {"Wooden Bolt", false},
        {"Wooden Bow", false},
        {"Wooden Bucket", false},
        {"Wooden Club", false},
        {"Wooden Crossbow", false},
        {"Wooden Fishing Rod", false},
        {"Wooden Ring", false},
        {"Wooden Shield", false}
    };
    public Dictionary<string, bool> DiscoveredItems { get { return discoveredItems; } }

    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI activeCount;
    [SerializeField] GameObject currentSpriteImage;

    Queue<string> activeNotificationsNameList = new Queue<string>();
    Queue<Sprite> activeNotificationSpritesList = new Queue<Sprite>();
    string currentActiveName;
    Sprite currentActiveSprite;

    Image currentSpriteTexture;
    float lifetime = 5f;
    bool isDisplaying;

    void Awake()
    {
        currentSpriteTexture = currentSpriteImage.GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void CreateDiscoveredNotification(string name, Sprite sprite)
    {
        discoveredItems[name] = true;
        activeNotificationsNameList.Enqueue(name);
        activeNotificationSpritesList.Enqueue(sprite);
        UpdateFirstInQueues();

        if (activeNotificationsNameList.Count > 1) //If another notification is currently showing:
        {
            StartCoroutine(WaitInQueue(name));
        }

        else
        {
            UpdateDiscoveredNotification();
        }
    }

    public void UpdateDiscoveredNotification()
    {
        itemName.text = currentActiveName;
        currentSpriteTexture.sprite = currentActiveSprite;
        StartCoroutine(SetLifetime());
    }

    public void UpdateFirstInQueues()
    {
        currentActiveName = activeNotificationsNameList.Peek();
        currentActiveSprite = activeNotificationSpritesList.Peek();

        int count = activeNotificationsNameList.Count - 1;

        if (count >= 1)
        {
            activeCount.text = $"(+{count})";
        }

        else
        {
            activeCount.text = null;
        }
    }

    public void RemoveFirstInQueues()
    {
        activeNotificationsNameList.Dequeue();
        activeNotificationSpritesList.Dequeue();
    }

    IEnumerator WaitInQueue(string name)
    {
        while (currentActiveName != name)
        {
            yield return null;
        }

        UpdateDiscoveredNotification();
    }

    IEnumerator SetLifetime()
    {
        yield return new WaitForSecondsRealtime(lifetime);
        RemoveFirstInQueues();

        if (activeNotificationsNameList.Count > 0) //If something else is queued:
        {
            UpdateFirstInQueues();
        }

        else
        {
            gameObject.SetActive(false);
        }
    }

    //Queue of activeDiscoveredNotifications
    //variable activeDiscoveredNotification = first item in this list
    //In craeteNotification, add to end of list. Start coroutine to display it
    //THis coroutine waits every frame until activeDiscoveredNotification is this Notification.
    //Then it starts the lifetime.
    //At end of lifetime, remove from list and set active to false.
}
