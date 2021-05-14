using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    [SerializeField] private bool _isPaused = false, isGOScreen = false;
    [SerializeField] private GameObject _panelPause;
    [SerializeField] private GameObject _saveloadButtons;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _mainCamera;
    private playerController _playerCtrl;
    private attack _playerAttack;
    private ThirdPersonCamera _mainCam;
    [SerializeField] private RainSound rainSound;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private WaterSound _waterSound;
    [SerializeField] private CutSceneManager _cutScene;
    [SerializeField] private GameObject _image;
    
    bool check = false;

    void Start()
    {
        _saveloadButtons.SetActive(false);
        _mainCam = _mainCamera.GetComponent<ThirdPersonCamera>();
        _playerCtrl = _player.GetComponent<playerController>();
        _playerAttack = _player.GetComponent<attack>();
        //LoadPlayer();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGOScreen)
        {
            _isPaused = !_isPaused;
            if (_cutScene.inCutScene && _isPaused)
            {
                _cutScene.PauseCutScene();
            } else if (_cutScene.inCutScene && !_isPaused)
            {
                _cutScene.ResumeCutScene();
            }
            
        }
        if (_isPaused)
        {
            
            if (check)
            {
                rainSound.StopSound();
                _waterSound.StopWaterSound();
                
                check = !check;
            }
            _panelPause.SetActive(true);
            _mainCam.enabled = false;
            _playerCtrl.inGamePlay = false;
            _playerAttack.enabled = false;
            if (!_cutScene.inCutScene)
            {
                _hud.gameObject.SetActive(false);
            }
            _image.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            if (_cutScene.inCutScene)
            {
                _cutScene.ResumeCutScene();
            }
            if (!check)
            {
                rainSound.PlaySound();
                _waterSound.PlayWaterSound();
                check = !check;
            }
            _mainCam.enabled = true;
            _playerAttack.enabled = true;
            _playerCtrl.inGamePlay = true;
            _panelPause.SetActive(false);
            if (!_cutScene.inCutScene)
            {
                _hud.gameObject.SetActive(true);
            }
            _image.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }
   /* public void SetPause(bool _value)
    {
        _isPaused = _value;
    }*/

    public void ActivateGOScreen()
    {
        isGOScreen = true;
           _isPaused = true;
        Cursor.visible = true;
        _gameOverScreen.SetActive(true);
    }
    public void Game()
    {
        isGOScreen = false;
        _isPaused = false;
        //_saveloadButtons.SetActive(true);
        _gameOverScreen.SetActive(false);
    }
   public void SavePlayer()
    {
        SaveSystem.SavePlayerData(_playerCtrl);
    }
      public void LoadPlayer()
      {
        Debug.Log("NOT WORKING");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        /*GameObject _plr = (GameObject)Resources.Load("testPlayerBones", typeof(GameObject));
        PlayerSaveData data = SaveSystem.LoadPlayerData();
        //_playerCtrl.health = data._health;
        _playerCtrl = _plr.GetComponent<playerController>();
        _playerAttack = _plr.GetComponent<attack>();
        _playerCtrl.SetHealth(data._health);
        _plr.GetComponent<Inventory>().SetCountOfMedKits(data._countOfMedKits);

        Vector3 pos;

        pos.x = data._playerPosition[0];
        pos.y = data._playerPosition[1];
        pos.z = data._playerPosition[2];
        //_player.transform.position = pos;
        Debug.Log(_playerCtrl.transform.position);
        Debug.Log("before");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //_playerCtrl.LoadPlayerInStart();
        //Game();
        Instantiate(_plr, pos, Quaternion.identity);
        Debug.Log("after");*/

        /*
          PlayerSaveData data = SaveSystem.LoadPlayerData();
          //_playerCtrl.health = data._health;
          Vector3 pos;
          _playerCtrl.isDead = false;
          pos.x = data._playerPosition[0];
          pos.y = data._playerPosition[1];
          pos.z = data._playerPosition[2];
          Debug.Log(pos);
          transform.position = pos;
          Debug.Log(transform.position);
          _playerCtrl.SetHealth(data._health);
          transform.GetComponent<Inventory>().SetCountOfMedKits(data._countOfMedKits);*/
    }
    public void Exit()
    {
        
        SceneManager.LoadScene(1);
    }
}
