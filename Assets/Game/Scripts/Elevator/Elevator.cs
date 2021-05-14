using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public void TurnElevator()
    {
        Debug.Log("dasdasd");
        this.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y + 180f, transform.rotation.z, 0);
    }
}
