using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    private RaycastHit _hit;
    private RaycastHit _hit2;
    private RaycastHit _hit3;
    [SerializeField] private bool LowCoverCheck = false;
    [SerializeField] private bool HighCoverCheck = false;
    [SerializeField] private int _typeOfWall;
    [SerializeField] private int _directionOfRay;
    [SerializeField] private int numberOfClicks;
    [SerializeField] private GameObject _wall;
    private playerController playerCtrl;
    private CharacterController controller;
    public bool CanVaultFromCover;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCtrl = GetComponent<playerController>();
        _directionOfRay = -1;
        _typeOfWall = -1;
        CanVaultFromCover = false;
    }

    
    void FixedUpdate()
    {
        
        LessRaysInIf();
        CheckCover();
        Hide();
        //DrawRays();

    }
    void MoveToCover(bool isHigh)
    {
        if (isHigh)
        {
            //controller.radius = 0.8f;

            if (playerCtrl.isCrouching)
            {
                playerCtrl.Crouch();
            }
        }
        Vector3 coverPosition = _hit3.point + _hit3.normal * .3f;
        //controller.Move(new Vector3(coverPosition.x, transform.position.y, coverPosition.z));
        transform.position = new Vector3(coverPosition.x, transform.position.y, coverPosition.z);
    }

    public void OutFromCover()
    {
            playerCtrl.inCover = false;
            Debug.Log("exit from cover");
            //controller.radius = 2f;
            CanVaultFromCover = false;
    }

    void HideAtCover(bool isHigh)
    {

        if (playerCtrl.inCover)
        {

            OutFromCover();
        }
        else
        {
            _wall = _hit.collider.gameObject;
            Debug.Log("Enter in cover");
            
            if (!playerCtrl.isCrouching && !isHigh)
            {
                playerCtrl.Crouch();
                CanVaultFromCover = true;
            }
            
            playerCtrl.inCover = true; //в плеерконтроллере надо сделать третий тип передвижения 300 строка 
            playerCtrl.stopMoveBetweenAiming();
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-_hit.normal), 1f);
             MoveToCover(isHigh);
        }
    }
    void Hide()
    {
        if (_typeOfWall != -1 && Input.GetKeyDown(KeyCode.Q))
        {
            switch (_typeOfWall)
            {
                case 0:
                    HideAtCover(false);
                    break;
                case 1:
                    HideAtCover(true);

                    break;
            }
        }
    }
    void DrawRays()
    {
        Debug.DrawRay(transform.position + Vector3.up * 1f, transform.forward * 1f, Color.blue);
        Debug.DrawRay(transform.position + Vector3.up * 2f, transform.forward * 1f, Color.blue);

        Debug.DrawRay(transform.position + Vector3.up * 1f, -transform.forward * 1f, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * 2f, -transform.forward * 1f, Color.red);

        Debug.DrawRay(transform.position + Vector3.up * 1f, transform.right * 1f, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 2f, transform.right * 1f, Color.green);

        Debug.DrawRay(transform.position + Vector3.up * 1f, -transform.right * 1f, Color.yellow);
        Debug.DrawRay(transform.position + Vector3.up * 2f, -transform.right * 1f, Color.yellow);



        /*
        Debug.DrawRay(transform.position + Vector3.up * 1.2f + transform.right * .4f, transform.forward * 1f, Color.white);
        Debug.DrawRay(transform.position + Vector3.up * 1.2f - transform.right * .4f, transform.forward * 1f, Color.white);
        */





    }

    void DrawHelpRays(Vector3 _direction, Vector3 _direction2)
    {
        _hit3 = _hit;
        for (int i = -1; i < 2; i++)
        {
             Vector3 targetOrigin = transform.position;
            targetOrigin += _direction2 * (i * .4f);
           // Debug.DrawRay(targetOrigin + Vector3.up * 1f, _direction * 1f, Color.green);
            if (Physics.Raycast(targetOrigin + Vector3.up * 1f, _direction, out _hit, 1f))
            {
                numberOfClicks++;
            }
            else
            {
                numberOfClicks = 0;
            }
        }
        if (numberOfClicks > 2)
        {
            
            numberOfClicks = 3;
            LowCoverCheck = true;
        }
        else
        {
            numberOfClicks = 0;
            LowCoverCheck = false;
        }
    }

    
    void LessRaysInIf()
    {// а я говорил


        if (Physics.Raycast(transform.position + Vector3.up * 1f, transform.forward, out _hit, 1f))
        {
            DrawHelpRays(transform.forward, transform.right);
        }
        else if (Physics.Raycast(transform.position + Vector3.up * 1f, -transform.forward, out _hit, 1f))
        {
            DrawHelpRays(-transform.forward, transform.right);
        }
        else if (Physics.Raycast(transform.position + Vector3.up * 1f, transform.right, out _hit, 1f))
        {
            DrawHelpRays(transform.right, transform.forward);
        }
        else if(Physics.Raycast(transform.position + Vector3.up * 1f, -transform.right, out _hit, 1f))
        {
            DrawHelpRays(-transform.right, transform.forward);
        }



        if (Physics.Raycast(transform.position + Vector3.up * 2f, transform.forward, out _hit2, 1f) ||
            Physics.Raycast(transform.position + Vector3.up * 2f, -transform.forward, out _hit2, 1f) ||
            Physics.Raycast(transform.position + Vector3.up * 2f, transform.right, out _hit2, 1f) ||
            Physics.Raycast(transform.position + Vector3.up * 2f, -transform.right, out _hit2, 1f))
        {
            HighCoverCheck = true;

        }
        else
        {
            HighCoverCheck = false;
        }
    } // лучше это не разворачивать
    void CheckCover()
    {
        if (LowCoverCheck)
        {
            if (HighCoverCheck)
            {
                if (_hit.collider.tag == "Cover")
                {
                    _typeOfWall = 1;
                }
            }
            else
            {
                if (_hit.collider.tag == "Cover")
                {
                    _typeOfWall = 0;
                }
            }
        }
        else
        {
            _typeOfWall = -1;
        }
    }
}

