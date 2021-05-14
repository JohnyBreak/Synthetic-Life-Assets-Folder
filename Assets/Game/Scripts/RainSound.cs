using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSound : MonoBehaviour
{
    public bool _canPlaySound;
    [SerializeField] private AudioSource rainSound;

   public void PlaySound()
    {
        if (_canPlaySound)
        {
            rainSound.Play();
        }
    }
    public void StopSound()
    {
        rainSound.Stop();
    }
}
