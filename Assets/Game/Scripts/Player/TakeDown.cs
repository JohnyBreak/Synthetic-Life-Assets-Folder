using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDown : MonoBehaviour
{

    [SerializeField] private playerController playerCtrl;
    private CharacterController controller;
    private StealthZone stealthZone = null;
    private Animator anim;
    private Animator enemyAnim;
    //private Transform enemyTransform;
    private GameObject enemy = null;
    public GameObject _grabPoint;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!playerCtrl.isAiming && !playerCtrl.isCrawling && !enemy.GetComponent<Enemy>().GetNeutralize() && !enemy.GetComponent<Enemy>().GetDeathStatus())
            {
                if (playerCtrl.canTakeDown)
                {
                    if (!playerCtrl.TakingDown)
                    {
                        if (playerCtrl.isCrouching)
                        {

                            playerCtrl.Crouch();

                        }
                        //playerCtrl.canPunch = false;
                        // playerCtrl.TakingDown = true;
                        
                        Grab();
                    }
                    else
                    {
                        //playerCtrl.canPunch = true;
                        // playerCtrl.TakingDown = false;
                        UnCouple();
                    }
                }
            }
        }

        if (playerCtrl.TakingDown)
        {
            enemyAnim.SetFloat("SpeedPercent", anim.GetFloat("SpeedPercent"));

            enemy.transform.position = _grabPoint.transform.position;

            if (Input.GetKeyDown(KeyCode.F))
            {
                NeutralizeEnemy();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                KillEnemy();
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.LeftShift) && !playerCtrl.isAiming)
        {
            playerCtrl.TakingDown = true;
            Grab();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerCtrl.TakingDown = false;
            UnCouple();
        }*/
    }

    /* public void SetEnemyTransform(Transform _transform)
     {
         enemyTransform = _transform;
     }*/

    public void SetEnemy(GameObject _enemyObject)
    {
        enemy = _enemyObject;
        enemyAnim = _enemyObject.GetComponent<Animator>();
        stealthZone = _enemyObject.GetComponentInChildren<StealthZone>();
        //Debug.Log(_enemyObject.name);
    }

    public void UnSetEnemy()
    {
        enemy.GetComponent<Enemy>().LeaveTheGrab();
        enemy.GetComponent<CharacterController>().radius = 1.4f;
        enemy = null;
        enemyAnim = null;
        stealthZone = null;
        //Debug.Log("UnSetEnemy");
    }
    void KillEnemy()
    {
        anim.SetBool("Kill", true);
        //Debug.Log("Kill");
        //UnCouple();
        enemy.GetComponent<Target>().TakeDownDeath();
    }

    void NeutralizeEnemy()
    {
        anim.SetBool("neutralize", true);

        //Debug.Log("Neutralize");
        //UnCouple();
        enemy.GetComponent<Target>().TakeDownNeutralize();

    }


    public void StopNeutralize()
    {
        anim.SetBool("neutralize", false);

        UnCouple();
    }

    public void StopKill()
    {
        anim.SetBool("Kill", false);

        UnCouple();
    }


    public void CoupleTogether()
    {
        //4.33f
        //Debug.Log("CoupleTogether");
        controller.radius = 2.2f;
        controller.center = new Vector3(controller.center.x, 4.23f /*4.65f*/, controller.center.z + 1f);
        enemy.GetComponent<Enemy>().SetInGrab(true);
        enemy.GetComponent<CharacterController>().radius = .3f;//1f;
        enemy.transform.parent = transform;
        transform.GetComponentInChildren<Weapon>().canShoot = false;
        transform.GetComponentInChildren<Weapon>().canReload = false;
        anim.SetBool("isTakingDown", true);
        enemyAnim.SetBool("isTakingDown", true);
    }
    void UnCouple()
    {
        playerCtrl.canPunch = true;
        playerCtrl.TakingDown = false;

        controller.radius = 1.7f;//2f;//1.4f;
        //Debug.Log("UnCouple");
        controller.center = new Vector3(controller.center.x, 4.23f, controller.center.z - 1f);
        //enemy.GetComponent<CharacterController>().radius = 1.4f;
        enemy.transform.parent = null;
        enemy.GetComponent<Enemy>().SetInGrab(false);
        transform.GetComponentInChildren<Weapon>().canShoot = true;
        transform.GetComponentInChildren<Weapon>().canReload = true;
        anim.SetBool("isTakingDown", false);
        enemyAnim.SetBool("isTakingDown", false);
        UnSetEnemy();
    }



    void Grab()
    {
        playerCtrl.canPunch = false;
        playerCtrl.TakingDown = true;
        if (playerCtrl.canTakeDown)
        {
            Vector3 RotationDirection = enemy.transform.position - transform.position;
            RotationDirection.y = transform.position.y;
            Quaternion lookdir = Quaternion.LookRotation(RotationDirection);
            //transform.position = enemyPosition + -enemyTransform.forward * 1f;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookdir, 5);
            stealthZone.RotateForTakeDown(transform);
            //Debug.Log("try to grab");
        }
        else
        {
            return;
        }
    }


}
