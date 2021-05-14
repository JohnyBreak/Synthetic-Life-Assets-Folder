using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static float Horizontal { get; private set; }
    public static float Vertical { get; private set; }
    public static float MouseVertical { get; private set; }
    public static float MouseHorizontal { get; private set; }

    public float mouseSensitivity = 1f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        MouseHorizontal = Input.GetAxis("Mouse X") * mouseSensitivity;
        MouseVertical = Input.GetAxis("Mouse Y") * mouseSensitivity;
    }
}
