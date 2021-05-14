using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class twocams : MonoBehaviour
{

    private Camera MainCamera;
    public Camera AimCamera;
    private playerController playerCtrl;
    bool check;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        MainCamera = Camera.main;
        playerCtrl = GetComponent<playerController>();
        check = true;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!playerCtrl.isGrounded)
        {
            CameraSwitch2();
        }

        if (playerCtrl.isGrounded && !playerCtrl.isCrawling && !playerCtrl.inCover && playerCtrl.inGamePlay && !playerCtrl.TakingDown)
        {
            if (Input.GetMouseButtonDown(1))
            {
                CameraSwitch();
            }
            if (Input.GetMouseButtonUp(1))
            {
                CameraSwitch();
            }
        }
        else
        {
                if (playerCtrl.camSwitch && playerCtrl.isGrounded)
                {
                    check = false;
                    MainCamera.enabled = false;
                    AimCamera.enabled = true;
                //playerCtrl.turnSmoothTime = 0.01f;
               
                }
                else
                {
                    check = true;
                    MainCamera.enabled = true;
                    AimCamera.enabled = false;
                
                //StartCoroutine(CameraSwitch2());
                //Invoke("CameraSwitch2", .5f);

            }
        }
    }
   /* private IEnumerator CameraSwitch2()
    {
       // Debug.Log("in");
        yield return new WaitForSeconds(5f);
        playerCtrl.turnSmoothTime = 0.1f;
        //Debug.Log("out");
    }*/

    void CameraSwitch()
    {
        MainCamera.enabled = !MainCamera.enabled;
        AimCamera.enabled = !AimCamera.enabled;
    }
    void CameraSwitch2()
    {
        MainCamera.enabled = true;
        AimCamera.enabled = false;
    }

}
