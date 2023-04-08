using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    GameObject deathCanvas;
    Vector3 playerSpawnPosition = new Vector3(152.6f, 2.5f, -23.5f);

    void Awake()
    {
        deathCanvas = GameObject.Find("DeathCanvas");
        deathCanvas.SetActive(false);
    }

    public void KillPlayer()
    {
        deathCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None; //unlock cursor
        Cursor.visible = true; //can see cursor
        gameObject.SetActive(false);
    }

    public void RespawnPlayer()
    {
        deathCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.transform.position = playerSpawnPosition;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        gameObject.SetActive(true);
    }
}
