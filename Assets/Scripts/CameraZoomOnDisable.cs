using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoomOnDisable : MonoBehaviour
{
    //Weapon Zoom variables
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] ArrowLauncher arrowLauncherScript;

    //---Zooms back out when switching from bow mid-draw---\\
    public void ZoomOutWhenDisabled() 
    {
        StartCoroutine(ZoomOut());
    }

    IEnumerator ZoomOut()
    {
        float zoomedOutFOV = arrowLauncherScript.ZoomedOutFOV;
        float zoomOutTime = arrowLauncherScript.ZoomOutTime;
        float onReleaseFOV = cinemachineCamera.m_Lens.FieldOfView;

        for (float i = 0; i <= zoomOutTime; i += Time.deltaTime)
        {
            cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(onReleaseFOV, zoomedOutFOV, i / zoomOutTime);
            yield return null;
        }
    }
}
