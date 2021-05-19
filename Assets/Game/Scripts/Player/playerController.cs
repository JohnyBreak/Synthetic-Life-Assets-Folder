using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[InitializeOnLoad]
public class playerController : MonoBehaviour
{
   // public static playerController Instance;
    //public int STATE = 0;
    public int maxHealth = 150;
    public int health;
    public bool holdALT = true;
    public bool isPunching = false;
    public bool inCover = false;
    public bool isCrouching = false;
    public bool camSwitch = false;
    public bool isAiming = false;
    public bool canAim;
    public bool isCrawling = false;
    public bool isShooting = false;
    public bool isDead = false;
    public bool canMove = true;
    public bool canPunch = true;
    public bool canTakeDown = false;
    public bool TakingDown = false;
    public float timer = 0;
    public float walkSpeed = 5f;
    public float runSpeed = 6f;
    public float slowWalkSpeed = 0.5f;
    [SerializeField]
    private float gravity;
    public float turnSmoothTime;
    [SerializeField]
    private float groundDistance;
    float turnSmoothVelocity;
    public float velocityY;
    bool check = true;
    public bool isGrounded;
    public bool inGamePlay;
    private int layerMask = 1 << 8;
    public LayerMask groundMask;
    RaycastHit hit;

    Vector3 velocity;
    //public Camera AimCamera;
    Vector3 inputDir;
    Vector3 input;
    public Transform groundCheck;
    //Cover cover;
    Transform cameraT;
    Animator animator;
    CharacterController controller;
    [SerializeField] private LayerMask _mask;
    private HealthBar healthBar;
    //[SerializeField] private GameObject _gameOverScreen;
    private Alarm _alarm;
    private Pause _pause;
    private Weapon _weapon;
    [SerializeField] private CutSceneManager _cutScene;
    [SerializeField] private AudioSource _deathSound;
    /* void Awake()
     {
         LoadPlayerInStart();
     }*/
    void Start()
    {
        _weapon = GetComponentInChildren<Weapon>();
        healthBar = GameObject.FindWithTag("Hud").GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);
        
        _alarm = GameObject.FindWithTag("Hud").GetComponentInChildren<Alarm>();
        _pause = GameObject.FindWithTag("PauseMenu").GetComponentInChildren<Pause>();
        inGamePlay = true;
        canPunch = true;
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;   //вырубаю курсор
        cameraT = Camera.main.transform;
        //cover = GetComponent<Cover>();
        controller = GetComponent<CharacterController>();
        layerMask = ~layerMask;


       // LoadPlayerInStart();


        /*if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);*/
    }


   /* void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position + transform.forward * 5f, _attackRange);

    }*/

    void Update()
    {

        if (inGamePlay && !_cutScene.inCutScene)
        {
            if (!isDead)
            {
                if (!isAiming && !TakingDown)
                {
                    NewCrouchBehaviour();
                }
                if (!isPunching)
                {
                    Move();
                }
                if (isCrawling)
                {
                    CrawlFirstPerson();
                }

                stopMoveBetweenAiming();

               /* if (Input.GetKeyDown(KeyCode.H))
                {
                    TakeDamage(20);
                }*/
            }
        }
    }

    public void Crouch()
    {
        if (!isCrouching && !isCrawling)
        {

            turnSmoothTime = 0.1f;
            controller.height = 5.54f;
            controller.center = new Vector3(controller.center.x, 3f/*2.85f*/, controller.center.z);
            isCrouching = true;
            animator.SetBool("isCrouching", isCrouching);

        }
        else
        {
            var cantStandUp = Physics.Raycast(transform.position, Vector3.up, out hit, 2f, layerMask);

            if (!cantStandUp && !isCrawling)
            {

                controller.height = 8f;
                controller.center = new Vector3(controller.center.x, 4.23f, controller.center.z);
                isCrouching = false;
                animator.SetBool("isCrouching", isCrouching);

            }
        }
        if (isCrawling)
        {
            var cantStandUp = Physics.Raycast(transform.position, Vector3.up, out hit, 1.3f, layerMask);

            if (!cantStandUp && isCrawling)
            {

                controller.height = 5.54f;
                controller.radius = 1.7f;//1.4f;//2f;
                controller.center = new Vector3(0, 3f/*2.85f*/, 0);
                isCrawling = false;
                animator.SetBool("isCrawling", isCrawling);
                isCrouching = true;
                animator.SetBool("isCrouching", isCrouching);
            }
        }
    }
    void CrawlFirstPerson()
    {
        //Debug.DrawRay(transform.position, Vector3.up * 1.2f, Color.blue);
        // Debug.DrawRay(transform.position, transform.forward * 1.2f, Color.blue);
        Vector3 origin = transform.position;//new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
        origin += Vector3.up * .5f;

        /*
        Debug.DrawRay(origin - transform.forward * .5f, transform.up * .5f, Color.red);
        Debug.DrawRay(origin + transform.forward * .5f, transform.up * .5f, Color.yellow);
        Debug.DrawRay(origin + transform.right * .5f, transform.up * .5f, Color.green);
        Debug.DrawRay(origin + transform.forward * .5f, transform.up * .5f, Color.yellow);
        Debug.DrawRay(origin - transform.right * .5f, transform.up * .5f, Color.blue);
        */

        var cantStandUpYellow = Physics.Raycast(origin + transform.forward * .5f, transform.up, out hit, .5f, layerMask);
        var cantStandUpRed = Physics.Raycast(origin - transform.forward * .5f, transform.up, out hit, .5f, layerMask);//Physics.Raycast(transform.position, transform.up, out hit, 1.2f, layerMask);
        var cantStandUpGreen = Physics.Raycast(origin + transform.right * .5f, transform.up, out hit, .5f, layerMask);
        var cantStandUpBlue = Physics.Raycast(origin - transform.right * .5f, transform.up, out hit, .5f, layerMask);

        if (cantStandUpRed && cantStandUpYellow && cantStandUpBlue && cantStandUpGreen)
        {
            //меняю камеру
            // Debug.Log("cam switch");
            camSwitch = true;
        }
        if (!cantStandUpRed && !cantStandUpYellow && !cantStandUpBlue && !cantStandUpGreen)
        {
            camSwitch = false;
        }

    }

    void NewCrouchBehaviour()
    {
        if (holdALT)
        {
            if (timer > 0.7)
            {
                Crawl();
                timer = 0;
                holdALT = false;
            }

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                timer += Time.deltaTime;
            }

        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            if (timer > 0.1 && timer < 0.7)
            {
                Crouch();
            }
            timer = 0;
            holdALT = true;
        }
    }

    void Crawl()
    {
        Debug.DrawRay(transform.position, Vector3.up * 1.5f, Color.blue);

        /* if (Input.GetKeyDown(KeyCode.X))
         {*/

        if (isCrawling == false /*&& isCrouching*/)
        {

            controller.height = 2.39f;
            controller.center = new Vector3(0, 1.45f/*1.22f*/, 0);//+ 2.5f);
            controller.radius = 1.3f;
            isCrawling = true;
            animator.SetBool("isCrawling", isCrawling);
            isCrouching = false;
            animator.SetBool("isCrouching", isCrouching);

        }
        else
        {
            var cantStandUp = Physics.Raycast(transform.position, Vector3.up, out hit, 2f, layerMask);

            if (cantStandUp && isCrawling) // НЕ НЕ МОЖЕТ встать, бедолага
            {
                Crouch();
            }
            else {

                controller.height = 8f;
                controller.radius = 1.7f;//1.4f;//2
                controller.center = new Vector3(0, 4.23f, 0);
                isCrawling = false;
                animator.SetBool("isCrawling", isCrawling);

            }
        }
        // }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        animator.SetBool("isDamaged", true);

        print("player health = " + health);
        if (health <= 0)
        {
            isDead = true;
            animator.SetBool("isDead", isDead);
            _deathSound.Play();
            isCrawling = true;
            _weapon.canShoot = false;
            Invoke("GOScreen", 2f);
        }
    }
    void GOScreen()
    {
        Debug.Log("lanlng;jnsa;n");
        _alarm.SetNormal();

        _pause.ActivateGOScreen();
    }
    public void SetHealth(int _health)
    {
        health = _health;
        healthBar.SetHealth(health);
    }

    public void Heal(int _health)
    {
        health += _health;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.SetHealth(health);
    }

    public void EndOfDamage()
    {
        animator.SetBool("isDamaged", false);
    }


    void FirstPersonMovement()
    {
        Vector3 RotationDirection = cameraT.forward;
        //print("напиши уже нормальное передвижение от первого лица, ну");
        float x = Input.GetAxis("Horizontal"); // передвижение в стороны - стрейфы
        float z = Input.GetAxis("Vertical"); // передвижение вперед-назад
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * Time.deltaTime * walkSpeed);
        Vector3 targetDir = RotationDirection;
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }
        Quaternion lookdir = Quaternion.LookRotation(targetDir);
        Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookdir, 1);
        transform.rotation = targetRot;
    }
    void ThirdPersonMovement()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputDir = input.normalized;

        if (inputDir != Vector3.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraT.eulerAngles.y;

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }
    }
    void TakingDownMovement()
    {
        /*
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(x, 0f, z).normalized;

        if (direction.magnitude >= 0.1f)
        {

            //float targetAngle
            controller.Move(direction * walkSpeed * Time.deltaTime);

        }*/
        Vector3 inputRotation = new Vector3(-Input.GetAxisRaw("Horizontal"), 0, -Input.GetAxisRaw("Vertical")).normalized;


        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputDir = input.normalized;

        if (inputDir != Vector3.zero)
        {
            float targetRotation = Mathf.Atan2(inputRotation.x, inputRotation.z) * Mathf.Rad2Deg + cameraT.eulerAngles.y;

            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }


        /*
        int _coeficient;
        Vector3 targetPos = cameraT.position;
        targetPos.y = transform.position.y;
        Vector3 targetAngleDir = targetPos - transform.position;
        Vector3 forward = transform.forward;
        float _angleBetwen = Vector3.SignedAngle(targetAngleDir, forward, Vector3.up);

        if (_angleBetwen > -90 && _angleBetwen < 90)
        {
            _coeficient = -1;
        }
        else
        {
            _coeficient = 1;
        }
        Vector3 input2 = new Vector3(Input.GetAxisRaw("Horizontal"), 0,Input.GetAxisRaw("Vertical"));
        Vector3 inputDir2 = input2.normalized;








        Vector3 targetPos2 = transform.position;
        targetPos.y = transform.position.y;
        Vector3 targetAngleDir2 = targetPos2 * 2f - inputDir2 * 2f;
        Vector3 forward2 = transform.forward;
        float _angleBetwen2 = Vector3.SignedAngle(targetAngleDir2, forward2, Vector3.up);

        Quaternion lookdir = Quaternion.LookRotation(inputDir2);
        Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookdir, 1);
        //transform.rotation = targetRot;

        /* Vector3 RotationDirection = cameraT.forward;

         float x = Input.GetAxis("Horizontal"); // передвижение в стороны - стрейфы
        // float z = Input.GetAxis("Vertical"); // передвижение вперед-назад
         Vector3 move = _coeficient * transform.right * x;
         controller.Move(move * Time.deltaTime * walkSpeed);
         Vector3 targetDir = RotationDirection;
         targetDir.y = 0;*/
        // Debug.Log(_angleBetwen);

    }

   /* void CoverMovement()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");



        Vector3 move = transform.right * horizontal + transform.forward * vertical;


        if (horizontal != 0)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (Physics.Raycast(transform.position + Vector3.up - transform.right * .4f, transform.forward, _mask))
                {
                    Debug.DrawRay(transform.position + Vector3.up - transform.right * .4f, transform.forward, Color.white);

                    Debug.Log("влево");
                    controller.Move(move * Time.deltaTime * walkSpeed);
                }
            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (Physics.Raycast(transform.position + Vector3.up + transform.right * .4f, transform.forward, _mask))
                {
                    Debug.DrawRay(transform.position + Vector3.up + transform.right * .4f, transform.forward, Color.white);

                    Debug.Log("вправо");
                    controller.Move(move * Time.deltaTime * walkSpeed);
                }

            }

        }
        if (vertical < -.8f)
        {
            cover.OutFromCover();
        }

        //input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //inputDir = input;//.normalized;

        if (move != Vector3.zero)
        {


            Debug.DrawRay(transform.position + transform.up * 1f, move, Color.blue);
            //float targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            move = move.normalized;
            // transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

        }
    }*/


    void RotationAndMovement() // вот здесь надо сделать третий тип для укрытия
    {
        if (isAiming)
        {
            FirstPersonMovement();
        }

        if (!isAiming)
        {
            if (/*isCrawling*/camSwitch)
            {
                FirstPersonMovement();
            }
            else
            {
                if (TakingDown)
                {
                    TakingDownMovement();

                }
                else
                {
                    /*if (inCover)
                    {
                        CoverMovement();

                    }
                    else
                    {

                        ThirdPersonMovement();
                    }*/
                    ThirdPersonMovement();
                }
            }
        }
    }
    public void stopMoveBetweenAiming()
    {
        if (Input.GetMouseButtonDown(1) || camSwitch || inCover)
        {
            // check = !check;
            inputDir = Vector3.zero;
        }
    }

    void Move()
    {
        if (canMove)
        {
            RotationAndMovement();
        }

        //CrawlMovement();


        bool running = Input.GetKey(KeyCode.LeftShift);
        bool step = Input.GetKey(KeyCode.LeftControl);

        float currentSpeed;

        if (running && !isCrouching && !isCrawling)
        {
            currentSpeed = runSpeed * inputDir.magnitude;
        }
        else
        {
            if (step)
            {
                currentSpeed = walkSpeed/*slowWalkSpeed*/ * inputDir.magnitude;
            }
            else
            {
                if (isCrouching || isCrawling || TakingDown)
                {
                    currentSpeed = walkSpeed * inputDir.magnitude;
                    
                }
                else
                {
                    currentSpeed = runSpeed/*walkSpeed*/ * inputDir.magnitude;
                }
            }
        }
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded /*&& velocity.y < 0*/)
        {
            //velocityY = 0;
            velocity.y = -5f;
            // Debug.Log("стоишь");

            animator.SetBool("isGrounded", true);
            canMove = true;
            canPunch = true;
            _weapon.canShoot = true;

        }
        else
        {
            animator.SetBool("isAiming", false);
            animator.SetBool("isGrounded", false);
            // Debug.Log("в воздухе");
            canMove = false;
            velocity.y += Time.deltaTime * gravity;
            isPunching = false;
            _weapon.canShoot = false;
        }

        if (TakingDown)
        {
            velocity = -transform.forward * currentSpeed + Vector3.up * velocity.y;
        }
        else
        {
            velocity = transform.forward * currentSpeed + Vector3.up * velocity.y;
        }
        //velocity = transform.forward * currentSpeed + Vector3.up * velocity.y;

        // падение
        controller.Move(velocity * Time.deltaTime);




        float animationSpeedPercent;
        if (running && !isCrouching && !isAiming && !isCrawling && !TakingDown)
            animationSpeedPercent = 1 * inputDir.magnitude;
        else
        {
            if (step || TakingDown && !isCrouching && !isAiming && !isCrawling)
                animationSpeedPercent = 0.5f * inputDir.magnitude;
            else
            {
                if (Input.anyKey == false)
                    animationSpeedPercent = 0.0f * inputDir.magnitude;
                else
                    animationSpeedPercent = 1f * inputDir.magnitude;
            }
        }
        animator.SetFloat("SpeedPercent", animationSpeedPercent);
    }

    public void LoadPlayerInStart()
    {
        PlayerSaveData data = SaveSystem.LoadPlayerData();
        //_playerCtrl.health = data._health;
        Vector3 pos;
        isDead = false;
        pos.x = data._playerPosition[0];
        pos.y = data._playerPosition[1];
        pos.z = data._playerPosition[2];
        Debug.Log(pos);
        transform.position = pos;
        Debug.Log(transform.position);
        SetHealth(data._health);
        transform.GetComponent<Inventory>().SetCountOfMedKits(data._countOfMedKits);
        
    }
}
