using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public Animator anim;
    public int noOfClicks = 0;
    private float lastClickedTime = 0;
    public float maxComboDelay;
    private playerController playerCtrl;
   // public float ShootSpeed;
   // public float ShootDelay = 0.0f;
   // public GunSystem gunSystem;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        playerCtrl = gameObject.GetComponent<playerController>();
        
    }


    void Update()
    {
        if (!playerCtrl.isAiming && playerCtrl.canPunch && !playerCtrl.inCover && !playerCtrl.TakingDown)
        {
            meleeAttack();
        }

        if (noOfClicks == 0)
        {
            return3();
        }
    }
    
    /*
    public void endOfShot()
    {
        playerCtrl.isShooting = false;
       // print("PieDiePew!");
        anim.SetBool("isShooting", playerCtrl.isShooting);
    }*/


    void meleeAttack()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if (!playerCtrl.isCrouching && !playerCtrl.isCrawling)
        {
            
                if (Input.GetMouseButtonDown(0))
                {
                    lastClickedTime = Time.time;
                    noOfClicks++;
                    if (noOfClicks == 1)
                    {
                        playerCtrl.isPunching = true;
                        anim.SetBool("Attack1", true);

                    // print("lastClickedTime" + lastClickedTime);
                    // print("noOfClicks" + noOfClicks);
                    
                    }
                    noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
                }
            
        }
    }
   public void return1()
    {
        
        if (noOfClicks >= 2)
        {
            playerCtrl.isPunching = true;
            anim.SetBool("Attack2", true);
            
        }
        else
        {
            anim.SetBool("Attack1", false);
            noOfClicks = 0;
            playerCtrl.isPunching = false;
        }
    }
    public void return2()
    {
        if (noOfClicks >= 3)
        {
            playerCtrl.isPunching = true;
            anim.SetBool("Attack3", true);
            
        }
        else
        {
            anim.SetBool("Attack2", false);
            anim.SetBool("Attack1", false);
            noOfClicks = 0;
            playerCtrl.isPunching = false;
        }
    }
    public void return3()
    {
        anim.SetBool("Attack1", false);
        anim.SetBool("Attack2", false);
        anim.SetBool("Attack3", false);
        noOfClicks = 0;
        playerCtrl.isPunching = false;
    }
}
