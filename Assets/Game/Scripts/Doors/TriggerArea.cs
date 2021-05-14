using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour  
{
    [SerializeField] private int id;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        GameEvents.current.DoorwayTriggerEnter(id);
        //print("ты в триггере");
    }
    private void OnTriggerExit(Collider other)
    {
        GameEvents.current.DoorwayTriggerExit(id);
        //print("ты вышел из триггера");
    }

}
