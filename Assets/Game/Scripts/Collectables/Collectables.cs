using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] private int _famasBullets = 60;
    [SerializeField] private int _pistolBullets = 20;
    [SerializeField] private int _typeOfBullets;
    [SerializeField] private int _typeOfCollectables;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Animator anim;
    private GameObject mainWeapon;
    private GameObject secondaryWeapon;
    private GameObject weaponHolder;
    private bool MaxValue;

    // Start is called before the first frame update
    void Start()
    {
        
        weaponHolder = GameObject.Find("weaponHolder");
        //weaponHolder.gameObject.GetComponent<WeaponSwitch>().secondaryWeapon;
        mainWeapon = GameObject.Find("MainWeapon");
        secondaryWeapon = weaponHolder.gameObject.GetComponent<WeaponSwitch>().secondaryWeapon;//GameObject.Find("SecondaryWeapon");//
    }
    /*
    // Update is called once per frame
    void Update()
    {
        
    }*/

    public void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "Player")
        {

            switch (_typeOfCollectables)
            {
                case 0: SendBullets();
                    break;
                case 1: SendMedKit();
                    break;
            }
            
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            MaxValue = false;
            anim.SetBool("MaxValue", MaxValue);
            //Debug.Log("exit");
        }
    }

    void SendMedKit()
    {

        if (inventory.GetCountOfMedKits() < inventory.GetMaxOfMedKits())
        {
            inventory.IncreaseMedKits();
            Destroy(gameObject);
        }
        else
        {
            //Debug.Log("enter");
             MaxValue = true;
            anim.SetBool("MaxValue", MaxValue);
        }
        
    }

    void SendBullets()
    {
        if (_typeOfBullets == 1)
        {

            if (mainWeapon.GetComponentInChildren<Weapon>().bulletsLeft < mainWeapon.GetComponentInChildren<Weapon>().GetMaxBullets())
            {
                mainWeapon.GetComponentInChildren<Weapon>().IncreaseBullets(60);
                // Debug.Log("в основном оружии осталось");
                // Debug.Log(mainWeapon.GetComponentInChildren<Weapon>().bulletsLeft);
                Destroy(gameObject);
            }
            else
            {
                //Debug.Log("enter");
                MaxValue = true;
                anim.SetBool("MaxValue", MaxValue);
            }

        }
        //Debug.Log("вошел в елсе");
        if (_typeOfBullets == 2)
        {
            if (mainWeapon.GetComponentInChildren<Weapon>().bulletsLeft < mainWeapon.GetComponentInChildren<Weapon>().GetMaxBullets())
            {
                secondaryWeapon.GetComponentInChildren<Weapon>().IncreaseBullets(20);
                // Debug.Log("в основном оружии осталось");
                // Debug.Log(mainWeapon.GetComponentInChildren<Weapon>().bulletsLeft);
                Destroy(gameObject);
            }
            else
            {
                //Debug.Log("enter");
                MaxValue = true;
                anim.SetBool("MaxValue", MaxValue);
            }
        }
    }


}
