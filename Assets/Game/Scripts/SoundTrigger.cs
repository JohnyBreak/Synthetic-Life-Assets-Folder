using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private WaterSound _waterSound;
    [SerializeField] private RainSound _rainSound;
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(this.id);
        if (other.tag == "Player")
        {
            if (id == 0)
            {
                _waterSound._canPlaySound = true;
                _rainSound._canPlaySound = true;
                _waterSound.PlayWaterSound();
                _rainSound.PlaySound();

            }
            if (id == 1)
            {
                _waterSound._canPlaySound = false;
                _rainSound._canPlaySound = false;
                _waterSound.StopWaterSound();
                _rainSound.StopSound();
            }
        }
    }
}
