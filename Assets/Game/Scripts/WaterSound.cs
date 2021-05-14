using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSound : MonoBehaviour
{
    public bool _canPlaySound;
    [SerializeField] private AudioSource[] collisionSounds;
    public void PlayWaterSound()
    {
        if (_canPlaySound)
        {
            foreach (AudioSource sound in collisionSounds)
            {
                sound.Play();
            }
        }
        
    }
    public void StopWaterSound()
    {
        foreach (AudioSource sound in collisionSounds)
        {
            sound.Stop();
        }
    }
}
