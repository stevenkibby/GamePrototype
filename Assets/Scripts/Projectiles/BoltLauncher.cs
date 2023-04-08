using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

public class BoltLauncher : MonoBehaviour
{
    //Input variable
    [SerializeField] InputActionReference attack;
    [SerializeField] TabInputManager tabInputManagerScript;
    
    //Bolt variables
    [SerializeField] float totalReloadTime = 1f;
    [SerializeField] float boltSpeed = 100f;
    Vector3 boltFireDirection = new Vector3(0f, 1f, 0f);
    GameObject currentBolt;
    GameObject currentEquippedBolts;
    float elapsedReloadTime = 0f;
    bool isLoaded = false;
    bool isReloading = false;
    int currentBoltIndex;
    int boltPoolLength;

    //UI variables
    [SerializeField] GameObject projectileCanvas;
    [SerializeField] Image blackBar;
    [SerializeField] Image greenBar;
    [SerializeField] Color lowColor;
    [SerializeField] Color fullColor;
    
    //Cache variables
    [SerializeField] GameObject objectPool;
    [SerializeField] ProjectileLifetime projectileLifetimeScript;
    ProjectileObjPool projectileObjPoolScript;
    Transform parent;
    Rigidbody[] boltRigidbodies;
    CapsuleCollider[] boltColliders;
    ProjectileCollision[] projectileCollisionScripts;
    GameObject[] boltPool;
    TrailRenderer[] projectileTrails;
    
    //Ammo variables
    [SerializeField] InventoryHandler inventoryHandlerScript;
    
    void Awake()
    {
        EnableUI(false);
        CreateBoltPool();
        Cache();
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if (CheckForBoltsInQuiver() == true) //If you have bolts equipped, load one of them
        {
            LoadBolt();
        } 
    }

    void OnDisable()
    {
        UnloadBolt();
    }

    void Update() 
    {
        if (attack.action.WasPressedThisFrame() && (tabInputManagerScript.CurrentlyTabbed == false)) //Click when not currently tabbed
        {
            if (isReloading)
            {
                Debug.Log("Currently loading bolt");
                return;
            }

            if (!isLoaded)
            {
                Debug.Log("You do not have any bolts currently equipped");
                return;
            }

            EnableBoltInteraction();
            LaunchBolt();

            if (CheckForBoltsInQuiver() == true) //If you have another bolt equipped, load one of them
            {
                LoadBolt();
            } 
        }
    }

    void EnableUI(bool value)
    {
        projectileCanvas.SetActive(value);
    }

    void CreateBoltPool()
    {
        projectileObjPoolScript = GetComponent<ProjectileObjPool>();
        boltPool = projectileObjPoolScript.CreateProjectileObjPool();
        boltPoolLength = boltPool.Length;
    }

    void Cache()
    {
        parent = transform;
        boltRigidbodies = new Rigidbody[boltPoolLength]; 
        boltRigidbodies = GetComponentsInChildren<Rigidbody>(true);
        boltColliders = new CapsuleCollider[boltPoolLength];
        boltColliders = GetComponentsInChildren<CapsuleCollider>(true);
        projectileCollisionScripts = new ProjectileCollision[boltPoolLength];
        projectileCollisionScripts = GetComponentsInChildren<ProjectileCollision>(true);
        projectileTrails = new TrailRenderer[boltPoolLength];
        for (int i = 0; i < boltPoolLength; i++)
        {
            projectileTrails[i] = projectileCollisionScripts[i].gameObject.GetComponentInChildren<TrailRenderer>(true); //error
        }
    }

    public void LoadBolt()
    {   
        if (gameObject.activeSelf)
        {
            StopAllCoroutines();
            StartCoroutine(LoadBoltCoroutine());
        }
    }

    public void UnloadBolt()
    {
        StopAllCoroutines();
        isLoaded = false;
        isReloading = false;
        elapsedReloadTime = 0f;

        if (projectileCanvas != null)
        {
            EnableUI(false);
        }

        if (currentBolt != null)
        {
            currentBolt.SetActive(false);
        }
    }

    IEnumerator LoadBoltCoroutine()
    {
        while (tabInputManagerScript.CurrentlyTabbed == true) //Keeps it from loading while tabbed
        {
            yield return null;
        }

        isReloading = true;
        elapsedReloadTime = 0f;
        EnableUI(true);

        while (elapsedReloadTime < totalReloadTime)
        {   
            elapsedReloadTime += Time.deltaTime;
            float chargePercent = elapsedReloadTime / totalReloadTime;
            greenBar.fillAmount = chargePercent;
            greenBar.color = Color.Lerp(lowColor, fullColor, chargePercent);
            yield return null;
        }

        elapsedReloadTime = 0f;
        EnableUI(false);
        SetBoltActive();

        while (!isLoaded) //Only called if bolt pool is currently depleted
        {
            Debug.Log("Bolt Pool Currently Depleted, Increase bolt pool size or reload time, as this should never call"); 
        }
    }

    void SetBoltActive()
    {
        for (int i = 0; i < boltPoolLength; i++)
        {
            if (!boltPool[i].activeInHierarchy)
            {
                boltPool[i].SetActive(true);
                isLoaded = true;
                isReloading = false;
                currentBolt = boltPool[i];
                currentBoltIndex = i;
                return;
            }
        }
    }

    void EnableBoltInteraction()
    {
        currentBolt.transform.SetParent(objectPool.transform);
        boltRigidbodies[currentBoltIndex].isKinematic = false;
        boltColliders[currentBoltIndex].enabled = true;
    }

    void LaunchBolt()
    {
        ProjectileCollision currentProjectileCollisionScript = projectileCollisionScripts[currentBoltIndex];
        TrailRenderer currentTrail = projectileTrails[currentBoltIndex];
        boltRigidbodies[currentBoltIndex].AddRelativeForce(boltFireDirection * boltSpeed);
        projectileLifetimeScript.ControlProjectileLife(currentBolt, currentProjectileCollisionScript, currentTrail, parent);
        currentEquippedBolts = inventoryHandlerScript.FindItemOfTypeAtEquippedIndex(2, ItemType.Bolt);
        inventoryHandlerScript.DecreaseStackableItem(currentEquippedBolts);
        isLoaded = false;
    }

    bool CheckForBoltsInQuiver()
    {
        currentEquippedBolts = inventoryHandlerScript.FindItemOfTypeAtEquippedIndex(2, ItemType.Bolt);

        if (currentEquippedBolts == null)
        {
            return false;
        }

        else
        {
            return true;
        }
    }


    //Click once to 
}