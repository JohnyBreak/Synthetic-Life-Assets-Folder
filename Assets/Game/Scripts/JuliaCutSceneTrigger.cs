using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class JuliaCutSceneTrigger : MonoBehaviour
{
    private playerController _playerCtrl;
    [SerializeField] private CutSceneManager _juliaCutScene;
    [SerializeField]
    private CutSceneManager _startCutScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<playerController>() != null)
        {
            _startCutScene.gameObject.SetActive(false);
            _juliaCutScene.gameObject.SetActive(true);

            _playerCtrl = other.GetComponent<playerController>();
            //other.GetComponent<Animator>().SetFloat("SpeedPercent", 0f);
            //_cutScene.PlayJuliaCutScene();
            //Destroy(this.gameObject);
            //Debug.Log("StartCutScene");
        }
        
    }
    public void EndGame()
    {
        SceneManager.LoadScene(1);
    }
}
