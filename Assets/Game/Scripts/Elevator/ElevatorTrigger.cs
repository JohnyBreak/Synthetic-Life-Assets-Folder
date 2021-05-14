using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    private playerController _playerCtrl;
    [SerializeField] private CutSceneManager _cutScene;
    [SerializeField] private GameObject _elevator;
    [SerializeField] private bool _turned = false;
    [SerializeField] private float _coef;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerCtrl = other.GetComponent<playerController>();
            //_cutScene.inCutScene = true;
            other.transform.SetParent(_elevator.transform);
            other.GetComponent<Animator>().SetFloat("SpeedPercent", 0f);
            _cutScene.PlayElevatorCutScene();
            Debug.Log("Enter");
            ChangeCoef();
        }
    }
    private void ChangeCoef()
    {
        if (_turned)
        {
            _coef = 0;
        }
        else
        {
            _coef = 180;
        }
        _turned = !_turned;
    }
    public void TurnElevator()
    {
        _elevator.gameObject.transform.rotation = Quaternion.Euler(0, _coef, 0);//new Quaternion(transform.rotation.x, transform.localRotation.y + _coef, transform.rotation.z, 0);
        _playerCtrl.gameObject.transform.rotation = Quaternion.Euler(0, _coef, 0);//new Quaternion(_playerCtrl.gameObject.transform.rotation.x, _playerCtrl.gameObject.transform.localRotation.y + _coef, _playerCtrl.gameObject.transform.rotation.z, 0);
        _playerCtrl.transform.SetParent(null);
        //this.gameObject.SetActive(false);
    }
   /* public void UnsetPlayerParent()
    {
        _playerCtrl.transform.SetParent(null);
    }*/
    
}
