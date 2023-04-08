using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    
    [SerializeField] int currentWeapon = 0;

    void Start()
    {
        SetWeaponActive();
    }

    void Update()
    {
        int previousWeapon = currentWeapon;

        ProcessKeyInput();
        ProcessScrollWheel();

        if (previousWeapon != currentWeapon)
        {
            SetWeaponActive();
        }
    }

    void SetWeaponActive()
    {
        int weaponIndex = 0;

        foreach (Transform weapon in transform)
        {
            if (weaponIndex == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
            }

            else
            {
                weapon.gameObject.SetActive(false);
            }

            weaponIndex ++;
        }
    }

    void ProcessKeyInput() // 1 and 2 keys with new input system
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 0;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = 1;
        }
    }

    void ProcessScrollWheel() //scroll wheel new input system
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0) //scrolling UP
        {
            if (currentWeapon >= transform.childCount - 1)
            {
                currentWeapon = 0;
            }

            else
            {
                currentWeapon ++;
            }
        }

        else if (Input.GetAxis("Mouse ScrollWheel") > 0) //scrolling DOWN
        {
            if (currentWeapon <= 0)
            {
                currentWeapon = transform.childCount - 1;
            }

            else
            {
                currentWeapon --;
            }
        }
    }
}
