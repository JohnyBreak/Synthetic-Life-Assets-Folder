using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private bool _patrolWaiting;
    [SerializeField] private float _totalWaitTime = 3f;
    [SerializeField] private bool _searchingTime;
    [SerializeField] private bool _inGrab;
    [SerializeField] private float _totalSearchingTime = 3f;
    [SerializeField] private float _switchProbabillity = 0.2f;
    [SerializeField] private List<Waypoint> _patrolPoints;



    // states
    [SerializeField] private bool _isDead = false;
    [SerializeField] private bool _inStan;
    [SerializeField] private bool _isNeutralized;
    [SerializeField] private bool _isPatroling;
    [SerializeField] private bool _isInvestigating;
    [SerializeField] private bool _isChasing;
    [SerializeField] private bool _isSearching;
    public bool _isAttacking;
    [SerializeField] private float _melleeAttackRange = 1.5f;
    [SerializeField] private float _attackRange = 10;
    [SerializeField] private bool _playerInMeleeAttackRange, _playerInAttackRange, _playerInViewArea;

    //info
    [SerializeField] private NavMeshAgent agent;
    public Transform playerTransform;
    public Vector3 lastPlayerPosition;
    [SerializeField] private LayerMask groundLayer, playerLayer;
    //[SerializeField] private Material stateMaterial;

    //patroling
    [SerializeField] private Vector3 _walkPoint;
    [SerializeField]  private float _walkPointRange;
    private bool _walkPointSet;
    

    //attacking
    [SerializeField] private float _timeBetweenAttacks;
    private bool _alreadyAttacked;

    // personal properties // base behaviour 
    [SerializeField] private float _health;
    [SerializeField] private int _currentPatrolIndex, _oldPatrolIndex;
    public bool _traveling;
    [SerializeField] private bool _waiting;
    [SerializeField] private bool _searching;
    private bool _patrolForward = true;
   // [SerializeField] private bool _alarm;
    private bool _grabAlarm, _grabAlarm2;
    [SerializeField] private bool _lookAround, _lookAround2;
    private int directionChoice;
    Vector3 lookSideTemp;
    private float _waitTimer, _rotateTimer;

    [SerializeField] private GameObject fieldOfView;
    public Alarm _alarm;
    private Animator animator;
    private CharacterController controller;
    private EnemyAttack _enemyAttack;
    
    public Enemy(int hp)
    {
        _health = hp;
    }

    void Start()
    {
        _enemyAttack = GetComponent<EnemyAttack>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        //playerTransform = GameObject.Find("bolvankaP1").transform;
        agent = GetComponent<NavMeshAgent>();
        _lookAround = true;
        //_alarmTrigger = transform.GetChild(5).transform.gameObject;


        if (agent == null)
        {
            Debug.Log("нет навмеш агента на объекте с именем " + gameObject.name);
        }
        else
        {
            if (_patrolPoints != null && _patrolPoints.Count > 1)
            {
                _currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.Log("недостаточно точек для патрулирования");
            }
        }
    }

    void Update()
    {
        if (_inStan)
        {
            animator.SetFloat("SpeedPercent", 0f);
        }
        if (!_isDead)
        {
            if (!_isNeutralized && !_inGrab && !_inStan)
            {
                   // _playerInMeleeAttackRange = Physics.CheckSphere(transform.position + transform.forward, _melleeAttackRange, playerLayer);
                    _playerInAttackRange = Physics.CheckSphere(transform.position + transform.forward * 5f, _attackRange, playerLayer);
                
                if (_grabAlarm)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        _grabAlarm = false;
                        //Debug.Log("вырвался пидор");
                        _grabAlarm2 = true;
                        agent.SetDestination(playerTransform.position);
                    }
                }
                if (_grabAlarm2)
                {
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        _grabAlarm2 = false;
                        //Debug.Log("ага, стоять пидор");
                        agent.SetDestination(transform.position);
                        agent.speed = 0;
                        animator.SetFloat("SpeedPercent", 0f);
                        if (!_playerInViewArea)
                        {
                            //ResetStateAfterNeutralize();
                            Invoke("ResetStateAfterNeutralize", 3.5f);
                            //_lookAround2 = true;
                            LookAroundAfterNeutralize();
                        }
                    }
                }
                if (_lookAround2)
                {
                    LookAroundAfterNeutralize();
                }
                if (_playerInAttackRange && _playerInViewArea)
                {
                    CheckForAttack();
                    SetStateToAttacking();
                }
                if (_isAttacking)
                {
                    _enemyAttack.Attack();
                }
                else
                {
                    _enemyAttack.StopAttack();
                }
                if (_playerInMeleeAttackRange)
                {
                    MelleeAttackPlayer();
                }
                if (_isChasing)
                {
                    Chase();
                }
                if (_isPatroling)
                {
                    Patrol();
                }
                if (_isSearching)
                {
                    Search();
                }
                if (_isInvestigating)
                {
                    Investigating();
                }
            }
            else
            {
                SetStateToLay();
            }
        }
        else
        {
            SetStateToLay();
        }
    }

    public void SetPlayerInViewArea(bool inArea)
    {
        /* if (inArea)
         {
             _alarm = true;
         }*/
        if (!_isNeutralized)
        {
            _playerInViewArea = inArea;
        }
        
    }
    
    void CheckForAttack()
    {
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (distance <= 10f)
        {
            StopForAttack();
        }
    }

    void Search()
    {
        
        if (_traveling && agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetFloat("SpeedPercent", 0f);
            _traveling = false;
            //agent.SetDestination(transform.position);
       
        if (_searchingTime)
        {
            _searching = true;
            _waitTimer = 0;
            //Debug.Log("_searchingTime");
        }
        }
        if (_searching)
        {
            if (_lookAround)
            {
                RandomLookSide();
            }

            LookAround(/*lookSideTemp*/);

            //Debug.Log("_searching");
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _totalSearchingTime)
            {
                //Debug.Log("if");
                _searching = false;

                SetStateToPatroling();
            }
        }
        
    }
    void Chase()
    {
        Vector3 chaseLookPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        _traveling = true;
        transform.LookAt(chaseLookPosition);
        lastPlayerPosition = playerTransform.position;
        agent.SetDestination(playerTransform.position);
    }
    void Investigating()
    {
        if (_traveling && agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetFloat("SpeedPercent", 0f);
            _traveling = false;
            SetStateToPatroling();
        }
        Vector3 chaseLookPosition = new Vector3(lastPlayerPosition.x, transform.position.y, lastPlayerPosition.z);
        _traveling = true;
        transform.LookAt(chaseLookPosition);
        agent.SetDestination(lastPlayerPosition);
    }

    void Patrol()
    {
        if (_traveling) animator.SetFloat("SpeedPercent", 0.5f);
        else animator.SetFloat("SpeedPercent", 0f);
        
        if (_traveling && agent.remainingDistance <= agent.stoppingDistance)//agent.remainingDistance <= 0.2f)
        {
            _traveling = false;

            if (_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }
        if (_waiting)
        {
            // looling around
            if (_lookAround)
            {
                RandomLookSide();
            }

            LookAround(/*lookSideTemp*/);
            
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _totalWaitTime)
            {
                _waiting = false;
                _lookAround = true;
                ChangePatrolPoint();
                SetDestination();
            }
        }
          /*  Vector3 direction = (_patrolPoints[_currentPatrolIndex].transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 9f);
        */
    }

    void LookAroundAfterNeutralize()
    {
        
        if (_lookAround)
        {
            _rotateTimer = 0;
            RandomLookSide();
            //_lookAround = false;
        }
       // Debug.Log("LookAroundAfterNeutralize");
        LookAround(/*lookSideTemp*/);

        _rotateTimer += Time.deltaTime;
        if (_rotateTimer >= _totalWaitTime - 1.5f)
        {
            _lookAround = true;
            _lookAround2 = false;
        }
    }

    void RandomLookSide()
    {
        System.Random randomDirection = new System.Random();
        directionChoice = randomDirection.Next(0, 3);
        //Debug.Log("random");
        // method call
        lookSideTemp = DetermineLookSide();

        _lookAround = false;
    }
    
    Vector3 DetermineLookSide()
    {
        if (directionChoice == 0)
        {
           // Debug.Log("Left side rotation");
            return transform.position - transform.right;
        }
        else if (directionChoice == 1)
        {
            //Debug.Log("Forward side rotation");
            return transform.position + transform.forward;
        }
        else
        {
            //Debug.Log("Right side rotation");
            return transform.position + transform.right;
        }
    }
    
    void LookAround(/*Vector3 lookSide*/)
    {
        
        Vector3 direction = (lookSideTemp - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);

    }

    void ChangePatrolPoint()
    {
       /* if(UnityEngine.Random.Range(0f, 1f) <= _switchProbabillity)
        {
            _patrolForward = !_patrolForward;
        }*/

        if (_patrolForward)
        {
            _currentPatrolIndex++;
            if (_currentPatrolIndex >= _patrolPoints.Count)
            {
                _currentPatrolIndex = 0;
            }
        }
        else
        {
            if (--_currentPatrolIndex < 0)
            {
                _currentPatrolIndex = _patrolPoints.Count - 1;
            }
        }
    }

    void SetDestination()
    {
        if (_patrolPoints != null)
        {
            Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;
            agent.SetDestination(targetVector);
            _traveling = true;
        }
    }
    
    public bool GetDeathStatus()
    {
        return _isDead;
    }

    public void SetDeathStatus(bool deathStatus)
    {
        _isDead = deathStatus;
    }
    public void SetInGrab(bool inGrab)
    {
        /*if (inGrab)
        {
            _alarm = true;
        }*/
        _inGrab = inGrab;
        fieldOfView.SetActive(!inGrab);
    }

    public void LeaveTheGrab()
    {
        _traveling = true;
        
        _inGrab = false;
        _grabAlarm = true;
        //fieldOfView.SetActive(true);
        animator.SetFloat("SpeedPercent", 0.9f);
        agent.speed = 3.3f;
        agent.SetDestination(playerTransform.position + playerTransform.forward * 2f);
        
    }
    
    public float GetHealth()
    {
        return _health;
    }

    public void SetHealth(float health)
    {
        _health = health;
    }
    public bool GetStan()
    {
        return _inStan;
    }

    public void SetStan(bool stan)
    {
       _inStan = stan;
    }
    public bool GetNeutralize()
    {
        return _isNeutralized;
    }

    public void SetNeutralize(bool neutralize)
    {
        _isNeutralized = neutralize;
        fieldOfView.SetActive(!neutralize);
    }
    public bool GetPatroling()
    {
        return _isPatroling;
    }
    public bool GetChasing()
    {
        return _isChasing;
    }
    
    public bool GetAtacking()
    {
        return _isAttacking;
    }

    public void SetStateToAttacking()
    {
        //
        _alarm.SetAlert();
        animator.SetFloat("SpeedPercent", 0f);
        _traveling = false;
        _isInvestigating = false;
        _isAttacking = true;
        _isPatroling = false; 
        _isChasing = false;
        _isSearching = false;
       // Debug.Log("атакую");
    }
    public void SetStateToPatroling()
    {
        if (_isChasing || _isSearching)
        {
            _currentPatrolIndex = _oldPatrolIndex;
        }
        SetDestination();
        agent.speed = 2f;
        //_alarm = false;
        _isInvestigating = false;
        _isPatroling = true;
        _isChasing = false;
        _isAttacking = false;
        _isSearching = false;
        //Debug.Log("патрулирую");
    }
    public void SetStateToChasing()
    {
        if (!_isNeutralized)
        {
            //
            _alarm.SetAlert();
            _oldPatrolIndex = _currentPatrolIndex;
            //agent.SetDestination(transform.position);
            agent.speed = 3f;
            animator.SetFloat("SpeedPercent", 0.9f);
            _isChasing = true;
            _isInvestigating = false;
            _isPatroling = false;
            _isAttacking = false;
            _isSearching = false;
            //Debug.Log("ищу");
        }
    }

    public void SetStateToInvestigating()
    {
        lastPlayerPosition = playerTransform.position;
        _waiting = false;
        _isSearching = false;
        _isInvestigating = true;
        _isChasing = false;
        _isPatroling = false;
        _isAttacking = false;
        agent.speed = 3f;
        animator.SetFloat("SpeedPercent", 0.9f);
    }

    public void SetStateToSearching()
    {
        
        agent.SetDestination(lastPlayerPosition);
        _traveling = true;
        animator.SetFloat("SpeedPercent", 0.9f);
        _isInvestigating = false;
        _isSearching = true;
        _isChasing = false;
        _isPatroling = false;
        _isAttacking = false;
        //Debug.Log("иду на последнюю точку");
    }

    void MelleeAttackPlayer()
    {

        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (distance <= agent.stoppingDistance)
        {
            StopForAttack();
        }

        //agent.SetDestination(transform.position);
        
        /*Vector3 chaseLookPosition = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        transform.LookAt(chaseLookPosition);*/
    }

    void StopForAttack()
    {
        animator.SetFloat("SpeedPercent", 0f);
        agent.SetDestination(transform.position);
        FaceTarget();
        _traveling = false;
    }
    
    void FaceTarget()
    {
        
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 9f);
    }

   /* void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
       //Gizmos.DrawWireSphere(transform.position + transform.forward, _melleeAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 90f);

    }*/
    
    public void ResetStateAfterNeutralize()
    {
        animator.SetFloat("SpeedPercent", 0f);
        _lookAround2 = true;
        Invoke("SetStateToPatroling", 7f);
        
    }
    
    void SetStateToLay()
    {

        _traveling = false;
        _isPatroling = false;
        _isChasing = false;
        _isAttacking = false;
        _isSearching = false;
        agent.SetDestination(transform.position);
        _oldPatrolIndex = _currentPatrolIndex;
    }
}
