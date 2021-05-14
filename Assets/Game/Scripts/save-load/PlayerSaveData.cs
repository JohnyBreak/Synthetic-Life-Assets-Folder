using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData 
{
    public int _health;
    public int _countOfMedKits;
    public float[] _playerPosition;
   
    public PlayerSaveData(playerController player)
    {
        _health = player.health;
        _countOfMedKits = player.GetComponent<Inventory>().GetCountOfMedKits();
        _playerPosition = new float[3];
        _playerPosition[0] = player.transform.position.x;
        _playerPosition[1] = player.transform.position.y;
        _playerPosition[2] = player.transform.position.z;
    }
}
