using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    Transform camHead;
    [SerializeField] Transform pivot;
    [SerializeField] private bool _startNextRotation = true;
    [SerializeField] private bool _rotRight;
    [SerializeField] private float _yaw;
    [SerializeField] private float _pitch;
    [SerializeField] private float _secondRotation;
    [SerializeField] private float _rotationSwitchTime;
    private CameraBreak cameraBreak;
    private CamDetection cameraDetection;
    private Transform testTransform;
    Vector3 test;
    private float _lastYRotation;

    void Start()
    {
        camHead = transform.GetChild(0);
        //testTransform = camHead.transform;
        //test = new Vector3(camHead.transform.rotation.x, camHead.transform.rotation.y, camHead.transform.rotation.z);
        camHead.transform.localRotation = Quaternion.Euler(_pitch, 0, 0);
        cameraDetection = gameObject.GetComponentInChildren<CamDetection>();
        cameraBreak = gameObject.GetComponentInParent<CameraBreak>();
        
        //camHead.localRotation = Quaternion.AngleAxis(_pitch, Vector3.right);
        SetUpStartRotation();
       
    }

    
    void Update()
    {
        
        if (cameraDetection.isAlarm)
        {
            
            LookAtPlayer();
            
        }
        else {
        
            if (_startNextRotation && _rotRight)
            {
                
                StartCoroutine(Rotate(_yaw, _secondRotation));
            }
            else if (_startNextRotation && !_rotRight)
            {
                
                StartCoroutine(Rotate(-_yaw, _secondRotation));
            }
        }
        _lastYRotation = pivot.transform.localRotation.y * 100;
       
      // Debug.Log(pivot.transform.localRotation.y);
    }
    
    public void StopLookAtPlayer()
    {
        Invoke("Waiter", 2f);
    }

    void Waiter()
    {
        if (cameraDetection.isAlarm)
        {
            return;
        }

        camHead.transform.localRotation = Quaternion.Euler(_pitch, 0, 0);
        pivot.transform.localRotation = Quaternion.Euler(0, 0, 0);
        _startNextRotation = true;

        SetUpStartRotation();
    }


    void LookAtPlayer()
    {
        _startNextRotation = false;
        _rotRight = false;
        if (camHead.transform.localRotation.y > -90 && camHead.transform.localRotation.y < 90)
        {
           camHead.transform.LookAt(cameraDetection.player.transform);
            
        }
        
    }

 

    IEnumerator Rotate(float _yaw, float _duration)
    {
        
        _startNextRotation = false;
        Quaternion initialRotation = pivot.transform.localRotation;
        float timer = 0;
        while (timer < _duration)
        {
            if (pivot.transform.localRotation.y > -0.72 || pivot.transform.localRotation.y < 0.72)
            {
                //Debug.Log("if");
                timer += Time.deltaTime;
                pivot.transform.localRotation = initialRotation * Quaternion.AngleAxis(timer / _duration * _yaw, Vector3.up);
                if (cameraBreak.isBroken || cameraDetection.isAlarm)
                {
                    yield break;
                }
                yield return null;
            }
            else
            {
                //Debug.Log("else");
                yield return null;
            }
        }
        yield return new WaitForSeconds(_rotationSwitchTime);

        _startNextRotation = true;
        _rotRight = !_rotRight;

    }
   /* void FallBrokenCamera(float _yaw, float _duration) // не работает
    {
        Debug.Log("enter");
        float timer = 0;
        while (timer < 1f)
        {
            Quaternion initialRotation = transform.rotation;
            timer += Time.deltaTime;
            transform.rotation = initialRotation * Quaternion.AngleAxis(timer / _duration * _yaw, Vector3.right);
        }
    }*/
    
        
    void SetUpStartRotation()
    {
        //_startNextRotation = true;
        if (_rotRight)
        {
            //Debug.Log("if");
            pivot.transform.localRotation = Quaternion.AngleAxis(-_yaw / 2, Vector3.up);
        }
        else
        {
            //Debug.Log("else");
            pivot.transform.localRotation = Quaternion.AngleAxis(_yaw / 2, Vector3.up);
        }
        
    }
}
