using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] private int countOfMedKits;
    [SerializeField] private int maxOfMedKits;
    [SerializeField] private playerController playerCtrl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (countOfMedKits > 0)
            {
                UseMedKit();
            }
            
        }

    }
    private void UseMedKit()
    {
        if (playerCtrl.health < playerCtrl.maxHealth)
        {
            countOfMedKits--;
            playerCtrl.Heal(40);
        }
        
    }
    public void IncreaseMedKits()
    {
        countOfMedKits++;
    }

    public int GetCountOfMedKits()
    {
        return countOfMedKits;
    }
    public void SetCountOfMedKits(int value)
    {
        countOfMedKits = value;
    }
    public int GetMaxOfMedKits()
    {
        return maxOfMedKits;
    }
}
