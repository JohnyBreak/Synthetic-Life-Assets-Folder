using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject mainWeapon;
    public GameObject secondaryWeapon;
    GameObject currentWeapon;
    Weapon wpn;
    public Animator anim;
    [SerializeField] private playerController playerCtrl;
    
    void Start()
    {

        secondaryWeapon.SetActive(false);
        //  wpn.gameObject.GetComponent<Weapon>();
        //gameObject.SetActive(false);
        currentWeapon = mainWeapon;
    }
    void Update()
    {
        //Debug.Log("mainWeapon - " + mainWeapon.GetComponent<Weapon>().currentBullets + "/" + mainWeapon.GetComponent<Weapon>().bulletsLeft);
        //Debug.Log("secondaryWeapon - " + secondaryWeapon.GetComponent<Weapon>().currentBullets + "/" + secondaryWeapon.GetComponent<Weapon>().bulletsLeft);

        if (playerCtrl.isGrounded && !playerCtrl.isCrawling && !playerCtrl.inCover && playerCtrl.inGamePlay && !playerCtrl.TakingDown)
            {
            if (Input.GetMouseButton(1))
            {
                currentWeapon.GetComponentInChildren<MeshRenderer>().enabled = true;
                //Debug.Log("mainWeapon - " + mainWeapon.GetComponent<Weapon>().currentBullets + "/" + mainWeapon.GetComponent<Weapon>().bulletsLeft);
            }
            else
            {
                currentWeapon.GetComponentInChildren<MeshRenderer>().enabled = false;
                //Debug.Log("secondaryWeapon - " + secondaryWeapon.GetComponent<Weapon>().currentBullets + "/" + secondaryWeapon.GetComponent<Weapon>().bulletsLeft);
            }

            SwitchWeapon();

        }
    }

    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetFloat("currentWeapon", 1);
            mainWeapon.SetActive(true);
            secondaryWeapon.SetActive(false);
            currentWeapon = mainWeapon;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetFloat("currentWeapon", 0);
            secondaryWeapon.SetActive(true);
            mainWeapon.SetActive(false);
            currentWeapon = secondaryWeapon;
        } 

        /* foreach (Transform weapon in transform)
         {
             weapon.gameObject.SetActive(!weapon.gameObject.activeSelf);
         }*/
    }


}
