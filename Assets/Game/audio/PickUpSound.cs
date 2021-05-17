using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSound : MonoBehaviour
{
    [SerializeField]private AudioSource _pickUpSound;
    public void PlayPickUpSound()
    {
        _pickUpSound.Play();
    }
}
