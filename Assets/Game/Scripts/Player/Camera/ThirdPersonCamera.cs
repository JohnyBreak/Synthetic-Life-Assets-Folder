using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private float hideDistance;
    public Camera cam;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform target;
    [SerializeField]private Transform _helper;
    [SerializeField] private float dstFromTarget;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    [SerializeField] private LayerMask obstacles;
    [SerializeField] private LayerMask noPlayer;
    [SerializeField] private LayerMask camOrigin;
    private Vector3 _localposition;
    [SerializeField] private float _maxDistance;
    [SerializeField] private playerController playerCtrl;
    [SerializeField] private CutSceneManager _cutScene;
    float distance;
    float yaw;
    float pitch;
    
    private Vector3 _position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    
    void Start()
    {
        _localposition = target.InverseTransformPoint(_position);
        _maxDistance = dstFromTarget + 1;//Vector3.Distance(_position, target.position);
        camOrigin = cam.cullingMask;
    }

    
    void LateUpdate()
    {
        if (!_cutScene.inCutScene)
        {
            rotateCam();
            if (!playerCtrl.isAiming)
            {
                Obstacles();
            }

            HidePlayer();
        }
       /* Debug.Log(Vector3.Distance(_position, target.position));
        Debug.DrawRay(target.position, transform.position - target.position, Color.blue, _maxDistance);
        Debug.DrawRay(transform.position, -transform.forward, Color.red, .5f);*/
        
        
    }
    
    void rotateCam()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        transform.eulerAngles = targetRotation;

        transform.position = target.position - transform.forward * dstFromTarget;
    }
    
    void Obstacles()
    {
        distance = Vector3.Distance(_position, target.position);
        RaycastHit hit;
        
        if (Physics.Raycast(target.position, transform.position - target.position, out hit, _maxDistance, obstacles))
        {
            _position = hit.point + transform.forward * .15f;
        }
        else
        {
            _position -= transform.forward;
        }
    }
    

    void HidePlayer()
    {
        var distance = Vector3.Distance(_position, target.position);
        if (distance < hideDistance)
        {
            cam.cullingMask = noPlayer;
        }
        else
        {
            cam.cullingMask = camOrigin;
        }
    }
}
