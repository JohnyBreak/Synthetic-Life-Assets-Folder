using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int _maxOfBullets;
    public float range;
    public int bulletsPerMagazine; // bullets in one magazine
    public int bulletsLeft; // total bullets we have 
    public int currentBullets; // how many bullets left in current mag
    public float damage, reloadTime;
    public float fireRate;
    public bool canShoot;
    public bool canReload;
    float fireTimer;
    public Transform fpsCam;
    public Animator anim;
    bool isShooting, reloading;
    private int bullets;
    private playerController playerCtrl;
    private AudioSource _shotSound;
    void Start()
     {
        _shotSound = GetComponent<AudioSource>();
        playerCtrl = GetComponentInParent<playerController>();
        currentBullets = bulletsPerMagazine;
        isShooting = false;
        canShoot = true;
        canReload = true;
     }

    
     void Update()
     {
            if (Input.GetMouseButton(1) && Input.GetMouseButton(0) && currentBullets > 0 && canShoot)
            {
                Fire();
            }
        
        
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.R) && !reloading && canReload)
            Reload();
    }
    void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Fire") || info.IsName("Crouch Fire"))
        {
            isShooting = false;
            anim.SetBool("isShooting", isShooting);
        }
    }
    public void Fire()
    {
        isShooting = true;
        if (fireTimer < fireRate) return;

        RaycastHit hit;

        if (Physics.Raycast(fpsCam.position, fpsCam.transform.forward, out hit, range))
        {
            // Debug.Log(hit.transform.name);
            if (hit.collider.CompareTag("Enemy"))// получаю коллайдер, в который попал
            {
                hit.collider.GetComponent<Target>().takeGunDamage(damage);//если это враг, посылаю ему урон
            }else if (hit.collider.tag == "SurveillanceCamera")// получаю коллайдер, в который попал
            {
                Debug.Log("SurveillanceCamera");
                hit.collider.GetComponentInParent<CameraBreak>().BreakCamera();//если это камера, ломаю
            }
                

        }

        anim.SetBool("isShooting", isShooting);
        _shotSound.Play();
        currentBullets--;
        fireTimer = 0.0f;
        
    }

    void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    void ReloadFinished()
    {
        bullets = bulletsPerMagazine - currentBullets; 

        if (bulletsLeft < 1)
        {
            Debug.Log("no ammo");
        }
        if (bulletsLeft >= bullets && bulletsLeft > 0)
        {
            currentBullets = bulletsPerMagazine;
            bulletsLeft -= bullets;
        }
        else
        {
            if (bulletsLeft < bullets && bulletsLeft > 0)
            {
                currentBullets += bulletsLeft;
                bulletsLeft -= bulletsLeft;
            }
        }
        
        Debug.Log("reloaded. bullets left " + currentBullets);
        reloading = false;
    }

    public int GetMaxBullets()
    {
        return _maxOfBullets;
    }
    public void IncreaseBullets(int _bullets)
    {
        bulletsLeft += _bullets;

        if (bulletsLeft > _maxOfBullets)
        {
            bulletsLeft = _maxOfBullets;
        }
    }

}
