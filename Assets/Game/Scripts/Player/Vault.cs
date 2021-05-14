using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vault : MonoBehaviour
{

    private RaycastHit _hit;
    private RaycastHit _groundHit;
    private float _range = 2;
    private float _vaultOverHeight = 2;
    [SerializeField] private playerController playerCtrl; 
    private int layerMask = 1 << 11;
    [SerializeField] private int _idOfObstacle = -1; // -1 нет препятствия, 0 "забор/укрытие", 1 низкое препятствие, 2 высокое препятствие
    
    [SerializeField] private int numberOfClicks;
    [SerializeField] private bool canMoveOngizmo;
    [SerializeField] private bool vaulting;
    [SerializeField] private bool canVault;
    
    [SerializeField] private bool canLowHanding;
    [SerializeField] private bool canHighHanding;
    [SerializeField] private LayerMask _mask;
    [Range(0,1)] [SerializeField] private float _time;
    private Cover _cover;
    private Vector3 p0;
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    private Vector3 p3;
    private Vector3 p4;
    private Vector3 _tempP1;
    private Vector3 _tempP2;
    private float _speed;
    [SerializeField] private float _coef;
    [SerializeField] private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        //_cover = GetComponent<Cover>();
       // Debug.Log(transform.gameObject.name);
    }


    void Update()
    {
        DrawRays();

        // _time += 0.03f * Time.deltaTime;
        // Debug.Log(_tempP1+ "  " + (_tempP1 + Vector3.up * 2f));
        
            if (canMoveOngizmo)
            {
            
            if (playerCtrl.isCrouching)
            {
                playerCtrl.Crouch();
            }
            playerCtrl.canMove = false;
                playerCtrl.canPunch = false;
            //Debug.Log("1");
                //transform.position = Bezier.GetPoint3(transform.position, p1.transform.position, p4, _time);
                transform.position = Bezier.GetPoint4(p0, _tempP1, _tempP2, p4, _time);
            //Debug.Log("2");
            _time += _speed * Time.deltaTime;
            }
        
        /*
        if (_cover.CanVaultFromCover)
        {
            canVault = true;
        }*/

        if (transform.position == p4)
        {
            _time = 0;
            canMoveOngizmo = false;
            anim.SetBool("isVault", false);
            playerCtrl.canMove = true;
            playerCtrl.canPunch = true;
           // Debug.Log("kek");
        }
        DefinitionOfObstacles();
    }
     private void OnDrawGizmos()
     {
         int _sigmentsNumber = 20;
         Vector3 _preveousPoint = transform.position;

         for (int i = 0; i < _sigmentsNumber + 1; i++)
         {
             float _parameter = (float)i / _sigmentsNumber;
             Vector3 _point = Bezier.GetPoint4(p0, _tempP1, _tempP2, p4, _parameter);
             Gizmos.DrawLine(_preveousPoint, _point);
             _preveousPoint = _point;
         }
     }


     void Vaulting(float _t, Vector3 p0, Vector3 _tempP1, Vector3 _tempP2, Vector3 p3)
    {
        playerCtrl.canMove = false;
        playerCtrl.canPunch = false;
        //transform.position = Bezier.GetPoint3(transform.position, p1.transform.position, p4, _time);
        transform.position = Bezier.GetPoint4(p0, _tempP1, _tempP2, p3, _time);
        //_time += _t * Time.deltaTime;
        //Debug.Log("lol");
    }

    void Jump()
    {
        switch (_idOfObstacle)
        {
            case 0:
                canMoveOngizmo = true;
                 Vaulting(1f, transform.position, p1.position, p2.position, _groundHit.point);
                p0 = transform.position; _tempP1 = p1.position; _tempP2 = p2.position; p4 = _groundHit.point;//Debug.Log("Vault");
                anim.SetBool("isVault", true);
                anim.SetFloat("VaultType", 0);
                anim.SetFloat("VaultSpeed", 2);
                _speed = .75f;
                break;
            case 1:
                canMoveOngizmo = true;
                //Vaulting(2f, transform.position, p1.position, p2.position, _groundHit.point);
                p0 = transform.position; _tempP1 = p1.position; _tempP2 = p2.position; p4 = _groundHit.point;
                anim.SetBool("isVault", true);
                anim.SetFloat("VaultType", 0.5f);
                anim.SetFloat("VaultSpeed", 1f);
                _speed = .75f;
                //Debug.Log("Low");
                break;
            case 2:
                canMoveOngizmo = true;
                //Vaulting(1f, transform.position, p1.position + Vector3.up * 1.5f + -transform.forward * 1f, _tempP2 = p2.position + Vector3.up * 1.5f + -transform.forward * .5f, _groundHit.point + -transform.forward * .5f);
                p0 = transform.position; _tempP1 = p1.position + Vector3.up * 1.5f + -transform.forward * 1f; _tempP2 = p2.position + Vector3.up * 1.5f + -transform.forward * .5f; p4 = _groundHit.point + -transform.forward * .5f;
                anim.SetBool("isVault", true);
                anim.SetFloat("VaultType", 1);
                anim.SetFloat("VaultSpeed", 1.5f);
                _speed = .75f;
                // Debug.Log("High");
                break;
        }
    }
void DefinitionOfObstacles()
    {
            if (canVault || canHighHanding || canLowHanding)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                if (!playerCtrl.isCrouching || !playerCtrl.isCrawling)
                {
                    Jump();
                    //playerCtrl.Crouch();
                }
                
                }
            }
    }



    void LandingPointCheck(Vector3 targetOrigin1)
    {
        
        Debug.DrawRay(targetOrigin1 + Vector3.up * 2.9f + transform.forward * 1.5f, -transform.up * 3f, Color.blue);
        
        Physics.Raycast(targetOrigin1 + Vector3.up * 2.9f + transform.forward * 1.5f, -transform.up, out _groundHit, 3f, _mask);
        bool check = (_groundHit.distance - 2.5f) == transform.position.y ? true : false;
       // Debug.Log("hit " + (_groundHit.distance - 2.5f) + "   my " + transform.position.y + "   check " + check);

        //Debug.Log((_groundHit.distance + transform.position.y) - transform.position.y);
        float difference = (_groundHit.distance + transform.position.y) - transform.position.y;
        Debug.Log(difference);
        if (difference > 2.5 && difference < 3)// 2.7 < > 2.3
        {
            canVault = true;
            _idOfObstacle = 0;
        }
        else 
        {
             if (difference > 0.8 && difference < 1.7)//0.9 < >1.1
             {
                _idOfObstacle = 1;
                canLowHanding = true;
             }
             else //-0,1 < > 0,1
             {
                _idOfObstacle = 2;
                canHighHanding = true;
             } 
        }


    }
       

    private void DrawRays()
    {

        Vector3 targetOrigin1 = transform.position;
        //float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;

        //Vector3 wallDirection = -_hit.normal * .5f;
        
        Debug.DrawRay(targetOrigin1 + Vector3.up * 1f, transform.forward * 1f, Color.red);
        if (Physics.Raycast(targetOrigin1 + Vector3.up * 1f, transform.forward, out _hit, 1f, layerMask))
        {
            for (int i = -1; i< 2; i++)
            {
            Vector3 targetOrigin = transform.position;
            targetOrigin += transform.right * (i * .3f);
                Debug.DrawRay(targetOrigin + Vector3.up * 1f, transform.forward * .8f, Color.green);
                if (Physics.Raycast(targetOrigin + Vector3.up * 1f, transform.forward, out _hit, .8f, layerMask))
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
                LandingPointCheck(targetOrigin1);
                numberOfClicks = 3;
            }
            else
            {
                _idOfObstacle = -1;
                canLowHanding = false;
                canHighHanding = false;
                canVault = false;
            }
        }
        else
        {
            _idOfObstacle = -1;
            canLowHanding = false;
            canHighHanding = false;
            numberOfClicks = 0;
            canVault = false;
        }


        /*
        Physics.Raycast(transform.position, Vector3.forward, out _hit, _range);
        RaycastHit vHit;
        Vector3 startOrigin = _hit.point;
        startOrigin.y = transform.position.y;
        Vector3 vOrigin = startOrigin + Vector3.up * _vaultOverHeight;*/



    }

}
