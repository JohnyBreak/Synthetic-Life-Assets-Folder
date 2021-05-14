using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioSource footSteps;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void FootSteps()
    {
        
        footSteps.Play();
    }

}
