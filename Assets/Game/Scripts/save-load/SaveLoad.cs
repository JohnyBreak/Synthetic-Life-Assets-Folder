using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveLoad : MonoBehaviour
{
    [SerializeField] private playerController _playerCtrl;
    public void SavePlayer()
    {
        SaveSystem.SavePlayerData(_playerCtrl);
    }


    public void LoadPlayer()
    {
       /* PlayerSaveData data = SaveSystem.LoadPlayerData();
        //_playerCtrl.health = data._health;
        _playerCtrl.SetHealth(data._health);
        _playerCtrl.GetComponent<Inventory>().SetCountOfMedKits(data._countOfMedKits);

        Vector3 pos;

        pos.x = data._playerPosition[0];
        pos.y = data._playerPosition[1];
        pos.z = data._playerPosition[2];
        transform.position = pos;
        Debug.Log(_playerCtrl.transform.position);
        Debug.Log(transform.position);
        Debug.Log("before");*/
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //_playerCtrl.LoadPlayerInStart();
        //Debug.Log("after");
    }
}
