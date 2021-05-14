using UnityEngine;
using System.Collections;
public class Target : MonoBehaviour
{
   // public float health;

    Animator animator;
    //public bool isDead = false;
    CharacterController controller;
    private EnemyAttack _enemyAttack;
    bool canTakeDamage;
    //[SerializeField] bool inStan;
    //public bool isNeutralized;
    private Enemy enemy;
    private float _health;
    private Transform playerTransform;
    [SerializeField] private Alarm _alarm;
    [SerializeField]
    private float _sleepTime;
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        _enemyAttack = GetComponent<EnemyAttack>();
        canTakeDamage = true;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        enemy = GetComponent<Enemy>();
        
    }

   /* public void takeDamage(float amount)
    {
        health -= amount;
        print("у цели осталось хп " + health);
        if (health <=0)
        {
           die();
        }
    }*/

    void die()
    {
        Destroy(gameObject);
    }

    public void takeMeleeDamage(float damage)
    {
        if (damage >= 25)
        {
            enemy.SetNeutralize(true);
            _enemyAttack.StopAttack();
            TranqSleep(_sleepTime);
        }
        else
        {
            _enemyAttack.StopAttack();
            animator.SetBool("isDamaged", true);
            enemy.SetStan(true);
            Debug.Log("враг в стане");
            StartCoroutine(StopStan());
        }
    }

    public void TakeDamage(float _damage)
    {
        _health -= _damage;
    }

    public float GetEnemyHealth()
    {
        return _health;
    }
    public void SetEnemyHealth(int _h)
    {
        _health = _h;
    }

    public void takeGunDamage(float damage)
    {
        if (damage > 0)
        {
            if (canTakeDamage)
            {
                if (!enemy._isAttacking)
                {
                    enemy.lastPlayerPosition = playerTransform.position;
                    enemy.SetStateToInvestigating();
                    //_alarm.SetAlert();
                }

                _enemyAttack.StopAttack();
                enemy.SetHealth(enemy.GetHealth() - damage);
                //health -= damage;
                canTakeDamage = false;
                animator.SetBool("isDamaged", true);

                print("enemy health = " + enemy.GetHealth());
                if (enemy.GetHealth() <= 0)
                {
                    enemy.SetDeathStatus(true);
                    animator.SetBool("isDead", enemy.GetDeathStatus());
                   /* controller.height = 2.39f;
                    controller.center = new Vector3(controller.center.x, 1.22f, controller.center.z);
                    controller.radius = 1.09f;*/

                    animator.SetBool("isDead", enemy.GetDeathStatus());
                    controller.height = .3f;
                    controller.center = new Vector3(controller.center.x, .5f/*1.22f*/, controller.center.z);
                    controller.radius = .42f;



                    Invoke("die", 8f);
                }
                Invoke("CanTakeDamage", 0.3f);
            }
        }
        else
        {
            
            Invoke("TranqSleepRef", 3f);
            //TranqSleep();
            animator.SetBool("isDamaged", true);
        }


        
    }
    
    void TranqSleepRef()
    {
        enemy.SetNeutralize(true);
        _enemyAttack.StopAttack();
        TranqSleep(_sleepTime);
        
    }
    
    public void StopNeutralized()
    {
        animator.SetBool("neutralize", false);
        controller.height = .3f;
        controller.center = new Vector3(controller.center.x, .5f/*1.22f*/, controller.center.z);
        controller.radius = .42f;
        TranqSleep(_sleepTime);
    }

    public void StopDeath()
    {
        animator.SetBool("Kill", false);
        
        animator.SetBool("isDead", enemy.GetDeathStatus());
        controller.height = .3f;
        controller.center = new Vector3(controller.center.x, .5f/*1.22f*/, controller.center.z);
        controller.radius = .42f;
    }
    
    IEnumerator StopStan()
    {
        yield return new WaitForSeconds(2f);
        enemy.SetStan(false);
        enemy.LeaveTheGrab();
        //Debug.Log("враг вышел из стана");
    }
    public void TakeDownNeutralize()
    {
        animator.SetBool("neutralize", true);
        enemy.SetNeutralize(true);

    }
    public void TakeDownDeath()
    {
        animator.SetBool("Kill", true);

        enemy.SetHealth(0);
        enemy.SetDeathStatus(true);

        Invoke("die", 8f);
    }

    void TranqSleep(float _time)
    {
        
        //Debug.Log("sleep");
        controller.height = .3f;
        controller.center = new Vector3(controller.center.x, .5f/*1.22f*/, controller.center.z);
        controller.radius = .42f;
        animator.SetBool("isDead", true);
        
        StartCoroutine(WakeUp(_time));
    }
    IEnumerator WakeUp(float _time)
    {
        //Debug.Log("enter");
        yield return new WaitForSeconds(_time);
        animator.SetBool("isDead", false);
        controller.height = 8f;
        controller.radius = 1.4f;
        controller.center = new Vector3(controller.center.x, 4.23f, controller.center.z);
        enemy.SetNeutralize(false);
        enemy.ResetStateAfterNeutralize();
        //Debug.Log("WakeUp");
    }
    void CanTakeDamage()
    {
        canTakeDamage = true;
    }
    public void EndOfDamage()
    {
        animator.SetBool("isDamaged", false);
    }
}
