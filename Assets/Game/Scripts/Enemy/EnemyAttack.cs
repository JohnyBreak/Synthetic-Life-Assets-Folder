using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int _damage; 
    public float fireRate, shootRate;
    float fireTimer, shootTimer;

    private Enemy _enemy;
    private Animator animator;
    [SerializeField] private AudioSource _shotSound;
    [SerializeField] private CharacterController _charCtrl;
    private playerController _playerCtrl;

    private Vector3 _shootPosition;
    void Start()
    {
        animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        //_playerCtrl = _enemy.playerTransform.GetComponent<CharacterController>();
        _playerCtrl = _charCtrl.GetComponent<playerController>();
    }

    
    void Update()
    {
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }
        if (shootTimer < shootRate)
        {
            shootTimer += Time.deltaTime;
        }
        
    }

    public void Attack()
    {
        animator.SetBool("isAiming", true);
        
        animator.SetBool("isShooting", true);
        _enemy.lastPlayerPosition = _enemy.playerTransform.position;
       
        
        Fire();
    }
    public void PlayShootSound()
    {
        _shotSound.Play();
    }
    void Fire()
    {
        if (fireTimer < fireRate) return;
        _shootPosition = transform.position + transform.up * 1.7f + transform.forward * 0.35f;
        //_playerCtrl = _enemy.playerTransform.GetComponent<CharacterController>();
        Vector3 target = _charCtrl.transform.position + _charCtrl.transform.transform.up * 1.7f;
        if (_playerCtrl.isCrouching)
        {
            target = _charCtrl.transform.position + _charCtrl.transform.transform.up * 1f;
        } else if (_playerCtrl.isCrawling)
        {
            target = _charCtrl.transform.position + _charCtrl.transform.transform.up * 0.5f;
        }
        //Debug.Log(target.x +" "+ target.y + " " + target.z);
        Vector3 direction = target - _shootPosition;
        RaycastHit hit;
        Debug.DrawRay(_shootPosition, direction * 70, Color.white);
        if (Physics.Raycast(_shootPosition, direction, out hit, 70))
        {
             //Debug.Log(hit.transform.name);
            if (hit.collider.CompareTag("Player"))// получаю коллайдер, в который попал
            {
                Debug.Log("есть пробитие");
                //_shotSound.Play();
                hit.collider.GetComponent<playerController>().TakeDamage(_damage);//если это игрок, посылаю ему урон
            }
        }
        
        fireTimer = 0.0f;
       
    }

    public void StopAttack()
    {

        _enemy._isAttacking = false;
        animator.SetBool("isAiming", false);
        animator.SetBool("isShooting", false);
    }

}
