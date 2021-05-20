using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDetection : MonoBehaviour
{
    Transform lens;
    public bool isAlarm = false;
    public bool playerInArea;
    [SerializeField] private Material searchingMat, spottedMat;
    [SerializeField] private float _distanse;
    Vector3 direction;
    Vector3 direction1;
    Vector3 direction2;
    private int _layerMask = 8;
    public GameObject player = null;
    private CamRotation cameraRotation;
    private CameraBreak _cameraBreak;
    [SerializeField] private Alarm _alarm;
    [SerializeField] private AlarmTrigger _alarmTrigger;
    void Start()
    {
        _cameraBreak = gameObject.GetComponentInParent<CameraBreak>();
        cameraRotation = gameObject.GetComponentInParent<CamRotation>();
        //lens.GetComponentInParent<Light>().color = Color.green;
        lens = transform.parent.GetComponent<Transform>();
        //_layerMask = ~_layerMask;
    }
    
    void Update()
    {
       /* if (isAlarm)
        {
            LookAtPlayer();
        }*/

    }
    void LookAtPlayer()
        {

        }
    void OnTriggerStay(Collider coll)
    {
        
        if (coll.gameObject.GetComponent<playerController>() != null)
        {
            if (!_cameraBreak.isBroken)
            {
            RaycastHit hit0;
            RaycastHit hit3;
            //Debug.DrawRay(lens.transform.position + lens.transform.forward * .3f, coll.transform.position + coll.transform.up * 1f - lens.transform.position, Color.blue, _distanse);
            Physics.Raycast(lens.transform.position + lens.transform.forward * .3f, coll.transform.position + coll.transform.up * 1f - lens.transform.position, out hit0, _distanse);
            Physics.Raycast(lens.transform.position + lens.transform.forward * .3f, coll.transform.position + coll.transform.up * 0.2f - lens.transform.position, out hit3, _distanse);
            //Debug.DrawRay(lens.transform.position + lens.transform.forward * .3f, coll.transform.position + coll.transform.up * 0.2f - lens.transform.position, Color.blue, _distanse);
            if (hit0.collider.gameObject.layer == _layerMask || hit3.collider.gameObject.layer == _layerMask)
            {
                playerInArea = true;

                player = coll.gameObject;
                direction = coll.transform.position + coll.transform.up * .5f - lens.transform.position;
                direction1 = coll.transform.position + coll.transform.up * 1.2f - lens.transform.position;
                direction2 = coll.transform.position + coll.transform.up * 0.2f - lens.transform.position;
                RaycastHit hit4;
                RaycastHit hit;
                RaycastHit hit1;
                Physics.Raycast(lens.transform.position + lens.transform.forward * .3f, direction1, out hit1, _distanse);
                Physics.Raycast(lens.transform.position + lens.transform.forward * .3f, direction, out hit, _distanse);
                Physics.Raycast(lens.transform.position + lens.transform.forward * .3f, direction2, out hit4, _distanse);

                /* if (Physics.Raycast(lens.transform.position, direction1, out hit1, _distanse) || Physics.Raycast(lens.transform.position, direction, out hit, _distanse))
                 {*/
                if (hit.collider.gameObject.layer == _layerMask || hit1.collider.gameObject.layer == _layerMask || hit4.collider.gameObject.layer == _layerMask)
                {

                    lens.GetComponentInParent<MeshRenderer>().material = spottedMat;
                    //Debug.Log(hit.collider.name);
                   // Debug.DrawRay(lens.transform.position + lens.transform.forward * .3f, direction, Color.red, _distanse);
                   // Debug.DrawRay(lens.transform.position + lens.transform.forward * .3f, direction1, Color.red, _distanse);
                    lens.GetComponentInParent<Light>().color = Color.red;
                    isAlarm = true;

                   // isGrounded = Physics.CheckSphere(lens.position, groundDistance, groundMask);


                    _alarm.SetAlert();
                    _alarmTrigger.setAlarm = true;
                }
                else
                {
                    
                    LeaveViewArea();

                }
                //}
            }
            else
            {
                
                LeaveViewArea();
            }



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
            //Debug.Log("LeaveViewArea");
            isAlarm = false;
            cameraRotation.StopLookAtPlayer();
            lens.GetComponentInParent<MeshRenderer>().material = searchingMat;
            lens.GetComponentInParent<Light>().color = Color.green;
        }

        
    }

}
