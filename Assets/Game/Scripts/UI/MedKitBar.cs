using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MedKitBar : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Text medKitDisplay;
    
    private int _medKitsLeft;
    

    void Update()
    {
        
        medKitDisplay.text = "MedKits Left: "+ inventory.GetCountOfMedKits().ToString();
        
    }
}
