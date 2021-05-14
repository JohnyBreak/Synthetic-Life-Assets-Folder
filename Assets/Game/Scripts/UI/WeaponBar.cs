using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponBar : MonoBehaviour
{

    [SerializeField] private WeaponSwitch weaponSwitch;
    [SerializeField] private Text ammoDisplay;
    private int currentBullets;
    private int bulletsLeft;
    [SerializeField] private playerController playerCtrl;

    void Update()
    {
        currentBullets = weaponSwitch.GetComponentInChildren<Weapon>().currentBullets;
        bulletsLeft = weaponSwitch.GetComponentInChildren<Weapon>().bulletsLeft;
        ammoDisplay.text = currentBullets.ToString() + "/" + bulletsLeft.ToString();

        if (playerCtrl.isAiming)
        {
            ammoDisplay.enabled = true;
        }
        else
        {
            ammoDisplay.enabled = false;
        }
    }
}
