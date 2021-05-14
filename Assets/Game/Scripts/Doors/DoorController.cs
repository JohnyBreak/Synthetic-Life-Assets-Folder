using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private int levelOfAcces;
    [SerializeField] private float up, down;
    [SerializeField] private AudioSource _doorSound;
    private void Start()
    {
        GameEvents.current.onDoorwayTriggerEnter += onDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit += onDoorwayClose;
    }

    private void onDoorwayClose(int id)
    {
        if (id == this.id)
        {
        LeanTween.moveLocalY(gameObject, down/*3f*/, 1f).setEaseInQuad();
            _doorSound.Play();
        }
        
    }

    private void onDoorwayOpen(int id)
    {

        if (id == this.id)
        {
            // LeanTween.moveLocalY(gameObject, 1.6f, 1f).setEaseOutQuad();
            // transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 5, transform.localPosition.z) ;
            // transform.localPosition = this.transform.localPosition + new Vector3(0f, 1f, 0f) * Time.deltaTime;
            
            LeanTween.moveLocalY(gameObject, up/*7f*/, 1f).setEaseOutQuad();
            _doorSound.Play();
                
        }
        
    }

}
