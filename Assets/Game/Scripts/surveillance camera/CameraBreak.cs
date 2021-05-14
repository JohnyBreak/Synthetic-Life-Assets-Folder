using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBreak : MonoBehaviour
{
    public bool isBroken = false;
    [SerializeField] private GameObject cameraHead;
    [SerializeField] private Transform cameraPivot;

    void Start()
    {/*
        camHead = transform.Find("CameraPivot");
        cameraBody = transform.Find("CameraHead").gameObject;
    */
    }
    
    void Update()
    {
        


        if (isBroken)
        {
            cameraPivot.GetComponent<CamRotation>().enabled = false;
            cameraHead.GetComponentInChildren<CamDetection>().enabled = false;
            cameraHead.GetComponentInChildren<Light>().enabled = false;
            cameraPivot.localRotation = Quaternion.AngleAxis(cameraPivot.transform.rotation.x + 70, Vector3.right);
            //cameraBody.SetActive(false);
        }
    }

    public void BreakCamera()
    {
        
        isBroken = true;
    }

}
