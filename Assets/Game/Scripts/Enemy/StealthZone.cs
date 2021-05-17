using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthZone : MonoBehaviour
{
    private bool _inZone;
    [SerializeField] private TakeDown takeDown;
    private GameObject enemyObject;
    private int _layerMask = 8;
    private GameObject grabPoint;
    [SerializeField] AudioSource _audioSourse;
    [SerializeField] AudioClip _deathSound;
    [SerializeField] AudioClip _neutralize;
    [SerializeField] AudioClip _grabSound;
    void Start()
    {

        grabPoint = takeDown._grabPoint;

    enemyObject = this.transform.parent.gameObject;
    }
    void Update()
    {
        // Debug.DrawRay(enemyObject.transform.position + enemyObject.transform.up, -enemyObject.transform.forward, Color.blue);
        if (_inZone)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //takeDown.SetEnemyTransform(transform.parent);
                
                takeDown.SetEnemy(enemyObject);
            }
        }
    }

    public void PlayGrabSound()
    {
        _audioSourse.PlayOneShot(_grabSound);
    }
    public void PlayGrabDeathSound()
    {
        _audioSourse.PlayOneShot(_deathSound);
    }
    public void PlayGrabNeutralizeSound()
    {
        _audioSourse.PlayOneShot(_neutralize);
    }
    public void RotateForTakeDown(Transform playerTransform)
    {
        Debug.Log("try to rotate enemy");

        Quaternion lookdir = Quaternion.LookRotation(playerTransform.forward);
        enemyObject.transform.rotation = Quaternion.Slerp(enemyObject.transform.rotation, lookdir, 1);


        enemyObject.transform.position = takeDown.transform.position;//playerTransform.position + playerTransform.forward * .25f;//.9f;
        takeDown.CoupleTogether();
    }



    public void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            RaycastHit hit;
            // Debug.Log("pew");
            Physics.Raycast(enemyObject.transform.position + enemyObject.transform.up, -enemyObject.transform.forward, out hit, 1f);
            //if (hit.collider.gameObject.tag == "Player")
           // {
                // Debug.Log("pew2222");
                _inZone = true;
                //takeDown.SetEnemyTransform(transform.parent);
                //takeDown.SetEnemy(enemyObject);
                Debug.Log("in stealth zone");
                collider.gameObject.GetComponent<playerController>().canTakeDown = true;

                //Debug.Log(transform.parent.position);
            //}
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //Debug.Log("out from stealth zone");
            _inZone = false;
            collider.gameObject.GetComponent<playerController>().canTakeDown = false;

        }
    }



}
