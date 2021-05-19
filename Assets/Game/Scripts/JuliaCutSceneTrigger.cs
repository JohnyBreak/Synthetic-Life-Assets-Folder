using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuliaCutSceneTrigger : MonoBehaviour
{
    private playerController _playerCtrl;
    [SerializeField] private CutSceneManager _cutScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerCtrl = other.GetComponent<playerController>();
            other.GetComponent<Animator>().SetFloat("SpeedPercent", 0f);
            //_cutScene.PlayJuliaCutScene();
            Destroy(this.gameObject);
        }
        
    }
}
