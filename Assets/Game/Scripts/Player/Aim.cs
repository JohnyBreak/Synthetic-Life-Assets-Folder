
using UnityEngine;

public class Aim : MonoBehaviour
{
    float turnSpeed = 750;
    private Transform Target;
    public Camera Cam;
    Vector3 CameraDir;
    public Vector3 Offset;
    public Vector3 HeadOffset;
    //public Camera Cam;
    private playerController playerCtrl;
    public Transform player;
    Transform spine;
    Transform head;
    Animator anim;
    public Canvas canvas;
    CharacterController charCtrl;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 rot;
   // Weapon wpn;
    void Start()
    {
        Target = GameObject.FindWithTag("MainCamera").transform;
        charCtrl = gameObject.GetComponent<CharacterController>();
        playerCtrl = gameObject.GetComponent<playerController>();
        anim = GetComponent<Animator>();
        spine = anim.GetBoneTransform(HumanBodyBones.Spine); /*Hips для всего тела, Spine только для верхней части. 
        в любом случае, сам ригид боди поворачивается в сторону нажатой кнопки wasd*/
        head = anim.GetBoneTransform(HumanBodyBones.Head);
        canvas.enabled = false;
       // wpn = gameObject.GetComponentInChildren<Weapon>();
    }
    
    void Aiming()
    {
        if (Input.GetMouseButton(1))
        {
           
            spine.LookAt(Target.position);
            spine.rotation *= Quaternion.Euler(Offset.x, Offset.y, Offset.z/*Offset.x, Offset.y, Offset.z*/); // поворачиваю торс
            playerCtrl.isAiming = true;
            anim.SetBool("isAiming", playerCtrl.isAiming);
            canvas.enabled = true;
            
            
        }

        if (Input.GetMouseButtonUp(1))
        {
            playerCtrl.isAiming = false;
            anim.SetBool("isAiming", playerCtrl.isAiming);
            canvas.enabled = false;
        }
    }
    void CrawlFirstPerson()
    {
        head.LookAt(Target.position);
        
        head.rotation *= Quaternion.Euler(HeadOffset.x, HeadOffset.y, HeadOffset.z); // поворачиваю голову
        //Debug.Log("тута вошел в башку");
    }
    void LateUpdate()
    {
        if (playerCtrl.inGamePlay)
        {
            if (playerCtrl.isGrounded && !playerCtrl.isCrawling && !playerCtrl.TakingDown && !playerCtrl.inCover)
            {
                Aiming();
            }
            if (playerCtrl.camSwitch)
            {
                CrawlFirstPerson();
            }
        }
        if (playerCtrl.isDead || !playerCtrl.isGrounded)
        {
            playerCtrl.isAiming = false;
            anim.SetBool("isAiming", playerCtrl.isAiming);
            canvas.enabled = false;
        }
    }
}
