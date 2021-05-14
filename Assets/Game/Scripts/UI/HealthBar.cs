using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int _health)
    {
        slider.maxValue = _health;
        slider.value = _health;
    }

    public void SetHealth(int _health)
    {
        slider.value = _health;
    }


/*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
