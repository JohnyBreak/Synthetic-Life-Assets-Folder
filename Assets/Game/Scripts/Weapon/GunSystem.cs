
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage, magaineSize, bulletsPerTap, range;
    public float timeBetweenShooting, reloadTime, timeBetweenShots;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //reference
    public Camera fpsCam;
    //public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask Enemy;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
       
        bulletsLeft = magaineSize;
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    void MyInput()
    {
       
        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft != magaineSize && !reloading)
            Reload();
        //Shoot
        if (Input.GetMouseButton(1))
        {
            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
                Shoot();
                
        }
    }



    void Shoot()
    {
            readyToShoot = false;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range, Enemy))
            {
               // Debug.Log(rayHit.collider.name);
                if (rayHit.collider.CompareTag("Enemy"))
                    rayHit.collider.GetComponent<Target>().takeGunDamage(damage);
            }
        Debug.Log("voshel");
        anim.SetBool("isShooting", !readyToShoot);
        
            Invoke("ResetShot", timeBetweenShooting);
        
    }

    void ResetShot()
    {
        bulletsLeft--;
        Debug.Log("bullets left "+bulletsLeft);
        readyToShoot = true;
        anim.SetBool("isShooting", !readyToShoot);
    }

    void Reload()
    {
        reloading = true;
        Invoke("ReloadFiniched", reloadTime);
    }
    void ReloadFiniched()
    {
        bulletsLeft = magaineSize;
        reloading = false;
        Debug.Log("reloaded. bullets left " + bulletsLeft);
    }

}
