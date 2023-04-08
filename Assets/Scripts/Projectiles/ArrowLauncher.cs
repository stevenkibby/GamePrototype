using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

public class ArrowLauncher : MonoBehaviour
{
    //Input variable
    [SerializeField] InputActionReference attack;
    [SerializeField] TabInputManager tabInputManagerScript;
    
    //Arrow variables
    Vector3 arrowFireDirection = new Vector3(0f, 1f, 0f);
    GameObject currentArrow;
    GameObject currentEquippedArrows;
    int currentArrowIndex;
    float currentArrowPower = 0f;
    float maxArrowPower = 30f;
    float arrowPowerFactor = 60f;

    //Weapon Zoom variables
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;

    float zoomedOutFOV = 60f;
    public float ZoomedOutFOV { get { return zoomedOutFOV; } }
    float zoomOutTime = 0.5f;
    public float ZoomOutTime { get { return zoomOutTime; } }

    float zoomedInFOV = 55f;

    //UI variables
    [SerializeField] GameObject projectileCanvas;
    [SerializeField] Image blackBar;
    [SerializeField] Image whiteBar;
    [SerializeField] Color lowColor;
    [SerializeField] Color fullColor;
    
    //Cache variables
    [SerializeField] GameObject objectPool;
    [SerializeField] ProjectileLifetime projectileLifetimeScript;
    ProjectileObjPool projectileObjPoolScript;
    Transform parent;
    Rigidbody[] arrowRigidbodies;
    CapsuleCollider[] arrowColliders;
    ProjectileCollision[] projectileCollisionScripts;
    GameObject[] arrowPool;
    int arrowPoolLength;
    TrailRenderer[] projectileTrails;
    

    //Script variables
    [SerializeField] InventoryHandler inventoryHandlerScript;
    [SerializeField] CameraZoomOnDisable cameraZoomOnDisableScript;
    
    void Awake()
    {
        EnableUI(false);
        CreateArrowPool();
        Cache();
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        UnnockArrow();

        // if (cameraZoomOnDisableScript == null) { return; }
        // cameraZoomOnDisableScript.ZoomOutWhenDisabled();
    }

    void Update() 
    {
        if (attack.action.WasPressedThisFrame() && (tabInputManagerScript.CurrentlyTabbed == false)) //Click when not currently tabbed
        {
            StartCoroutine(DrawArrow());
        }
    }

    void EnableUI(bool value)
    {
        projectileCanvas.SetActive(value);
    }

    void CreateArrowPool()
    {
        projectileObjPoolScript = GetComponent<ProjectileObjPool>();
        arrowPool = projectileObjPoolScript.CreateProjectileObjPool();
        arrowPoolLength = arrowPool.Length;
    }

    void Cache()
    {
        parent = transform;
        arrowRigidbodies = new Rigidbody[arrowPoolLength]; 
        arrowRigidbodies = GetComponentsInChildren<Rigidbody>(true);
        arrowColliders = new CapsuleCollider[arrowPoolLength];
        arrowColliders = GetComponentsInChildren<CapsuleCollider>(true);
        projectileCollisionScripts = new ProjectileCollision[arrowPoolLength];
        projectileCollisionScripts = GetComponentsInChildren<ProjectileCollision>(true);
        projectileTrails = new TrailRenderer[arrowPoolLength];
        for (int i = 0; i < arrowPoolLength; i++)
        {
            projectileTrails[i] = projectileCollisionScripts[i].gameObject.GetComponentInChildren<TrailRenderer>(true); //error
        }
    }

    IEnumerator DrawArrow()
    {
        if (CheckForArrowsInQuiver() == false) //Gets current arrows in quiver or stops coroutine if there are none.
        {
            Debug.Log("You do not have any arrows currently equipped.");
            yield break;
        }

        if (NockArrow() == false) //Enables arrow in pool. If pool is depleted, stops coroutine.
        {
            Debug.Log("Arrow pool currently depleted");
            yield break;
        }

        EnableUI(true);
        float onDrawFOV = cinemachineCamera.m_Lens.FieldOfView;

        while (attack.action.IsPressed())
        {
            if (currentArrowPower < maxArrowPower)
            {
                currentArrowPower += arrowPowerFactor * Time.deltaTime;
                
                if (currentArrowPower >= maxArrowPower)
                {
                    currentArrowPower = Mathf.Clamp(currentArrowPower, 0f, maxArrowPower);
                }

                float chargePercent = currentArrowPower / maxArrowPower;
                cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(onDrawFOV, zoomedInFOV, chargePercent);

                whiteBar.fillAmount = chargePercent;
                whiteBar.color = Color.Lerp(lowColor, fullColor, chargePercent);
            }

            yield return null;
        }

        EnableUI(false);
        //StartCoroutine(ZoomOut());
        EnableArrowInteraction();
        LaunchArrow();
    }

    public void UnnockArrow()
    {
        StopAllCoroutines();
        //StartCoroutine(ZoomOut());
        currentArrowPower = 0f;

        if (projectileCanvas != null)
        {
            EnableUI(false);
        }

        if (currentArrow != null)
        {
            currentArrow.SetActive(false);
        }
    }

    IEnumerator ZoomOut()
    {
        float onReleaseFOV = cinemachineCamera.m_Lens.FieldOfView;

        for (float i = 0; i <= zoomOutTime; i += Time.deltaTime)
        {
            cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(onReleaseFOV, zoomedOutFOV, i / zoomOutTime);
            yield return null;
        }
    }
    
    void EnableArrowInteraction()
    {
        currentArrow.transform.SetParent(objectPool.transform);
        arrowRigidbodies[currentArrowIndex].isKinematic = false;
        arrowColliders[currentArrowIndex].enabled = true;
    }

    void LaunchArrow()
    {
        ProjectileCollision currentProjectileCollisionScript = projectileCollisionScripts[currentArrowIndex];
        TrailRenderer currentTrail = projectileTrails[currentArrowIndex];
        arrowRigidbodies[currentArrowIndex].AddRelativeForce(arrowFireDirection * currentArrowPower);
        projectileLifetimeScript.ControlProjectileLife(currentArrow, currentProjectileCollisionScript, currentTrail, parent);
        currentArrowPower = 0f;
        currentArrow = null;
        inventoryHandlerScript.DecreaseStackableItem(currentEquippedArrows);
    }

    bool CheckForArrowsInQuiver()
    {
        currentEquippedArrows = inventoryHandlerScript.FindItemOfTypeAtEquippedIndex(2, ItemType.Arrow);

        if (currentEquippedArrows == null)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    bool NockArrow()
    {
        for (int i = 0; i < arrowPoolLength; i++)
        {
            if (!arrowPool[i].activeInHierarchy)
            {
                arrowPool[i].SetActive(true);
                currentArrow = arrowPool[i];
                currentArrowIndex = i;
                return true;
            }
        }

        Debug.Log("Arrow Pool Currently Depleted");
        return false;
    }
}