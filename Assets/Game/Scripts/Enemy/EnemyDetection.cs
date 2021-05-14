using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private bool playerInArea;
    //[SerializeField] private Material searchingMat, spottedMat;
    [SerializeField] private float _distanse;
    [SerializeField] private Enemy _enemy;
    Vector3 direction;
    Vector3 direction1;
    Vector3 direction2;
    Vector3 direction3;
    private int _layerMask = 8;
    private playerController _player;
    

    void Start()
    {
        _enemy = transform.GetComponentInParent<Enemy>();
        //head = transform.GetComponent<Transform>();
    }
    void OnTriggerStay(Collider coll)
    {

        if (coll.gameObject.tag == "Player")
        {
            _player = coll.GetComponent<playerController>();
            
            //Debug.Log("langdksgk");
            RaycastHit hit0;
            RaycastHit hit3;
            RaycastHit hit5;
            //Debug.DrawRay(head.transform.position + head.transform.forward*.3f, coll.transform.position + coll.transform.up * 1f - head.transform.position, Color.blue, _distanse);
            Physics.Raycast(head.transform.position + head.transform.forward * .3f, coll.transform.position + coll.transform.up * 1f - head.transform.position, out hit0, _distanse);
            Physics.Raycast(head.transform.position + head.transform.forward * .3f, coll.transform.position + coll.transform.up * 0.2f - head.transform.position, out hit3, _distanse);
            //Debug.DrawRay(head.transform.position + head.transform.forward * .3f, coll.transform.position + coll.transform.up * 0.2f - head.transform.position, Color.blue, _distanse);
            Physics.Raycast(head.transform.position + head.transform.forward * .3f, coll.transform.position + coll.transform.up * 1.5f - head.transform.position, out hit5, _distanse);
            //Debug.DrawRay(head.transform.position + head.transform.forward * .3f, coll.transform.position + coll.transform.up * 1.5f - head.transform.position, Color.blue, _distanse);


            if (hit0.collider.gameObject.layer == _layerMask || hit3.collider.gameObject.layer == _layerMask || hit5.collider.gameObject.layer == _layerMask)
            {
                playerInArea = true;
                _enemy.SetPlayerInViewArea(playerInArea);

                direction = coll.transform.position + coll.transform.up * .5f  - head.transform.position;
                direction1 = coll.transform.position + coll.transform.up * 1.2f - head.transform.position;
                direction2 = coll.transform.position + coll.transform.up * 0.2f - head.transform.position;
                direction3 = coll.transform.position + coll.transform.up * 1.5f - head.transform.position;

                RaycastHit hit4;
                RaycastHit hit;
                RaycastHit hit1;
                RaycastHit hit6;
                Physics.Raycast(head.transform.position + head.transform.forward * .3f, direction1, out hit1, _distanse);
                Physics.Raycast(head.transform.position + head.transform.forward * .3f, direction, out hit, _distanse);
                Physics.Raycast(head.transform.position + head.transform.forward * .3f, direction2, out hit4, _distanse);
                Physics.Raycast(head.transform.position + head.transform.forward * .3f, direction3, out hit6, _distanse);

                //Debug.Log("enter if");
                if (hit.collider.gameObject.layer == _layerMask || hit1.collider.gameObject.layer == _layerMask || hit4.collider.gameObject.layer == _layerMask || hit6.collider.gameObject.layer == _layerMask)
                {
                    
                    //Debug.Log(hit.collider.name);
                     //Debug.Log(hit4.collider.name);
                    Debug.DrawRay(head.transform.position + head.transform.forward * .3f, direction, Color.red, _distanse);
                    Debug.DrawRay(head.transform.position + head.transform.forward * .3f, direction1, Color.red, _distanse);
                    Debug.DrawRay(head.transform.position + head.transform.forward * .3f, direction2, Color.red, _distanse);
                    Debug.DrawRay(head.transform.position + head.transform.forward * .3f, direction3, Color.red, _distanse);
                    if (_player.health > 0)
                    {
                        _enemy.SetStateToChasing();
                    }
                    else
                    {
                        LeaveViewArea();
                        //Debug.Log("смэрть");
                    }
                }
                else
                {
                    //Debug.Log("enter 1 else");
                    LeaveViewArea();
                }
            }
            else
            {
                //Debug.Log("enter 2 else");
                LeaveViewArea();
            }
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            LeaveViewArea();
        }
    }
    void LeaveViewArea()
    {
        if (playerInArea)
        {
            playerInArea = false;
            _enemy.SetPlayerInViewArea(playerInArea);
            _enemy.SetStateToSearching();
            //Debug.Log("LeaveViewArea");
        }
    }
}
