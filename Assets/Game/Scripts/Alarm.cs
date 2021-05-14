using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Alarm : MonoBehaviour
{
    [SerializeField] private enum States
    {
        Normal, Search, Alert
    }
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private States _state;
    [SerializeField] private float _timer = 0, _alertTimer, _searchTimer, _alertInerTimer, _searchInerTimer;
    [SerializeField] private bool isAlertTimer = false, isSearchTimer = false;
    [SerializeField] private GameObject _alertLabel, _evasionLabel;
    [SerializeField] private Text textTimer;
    private AudioSource _alertSound;
    private bool _soundCheck = false;
    
    void Start()
    {
        _alertSound = GetComponent<AudioSource>();
        textTimer.text = "55:33";
        _alertInerTimer = _alertTimer / 100;
        _searchInerTimer = _searchTimer / 100;
    }
    void Update()
    {
        if (isAlertTimer)
        {
            if (_timer < _alertTimer)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                isAlertTimer = false;
                SetEvasion();
                _timer = 0f;
            }
        }
        if (isSearchTimer)
        {
            if (_timer < _searchTimer)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                isSearchTimer = false;
                SetNormal();
                _timer = 0f;
            }
        }
        //Debug.Log(_state);
        /*
        if (Input.GetKeyDown(KeyCode.I))
            _state = States.Normal;
        if (Input.GetKeyDown(KeyCode.O))
            _state = States.Search;
        if (Input.GetKeyDown(KeyCode.P))
            _state = States.Alert;
        */
    }
    public void PlayAlertSound()
    {
        //Debug.Log("ПРОИГРЫВАЕМ МУЗЫКУ НАХУЙ БЛЯТЬ!!!!");
        _alertSound.Play();
    }
    
    public  void SetAlert()
    {
        
        if (!_soundCheck)
        {
            _soundCheck = true;
            PlayAlertSound();
        }
        //PlayAlertSound();
        isAlertTimer = true;
        isSearchTimer = false;
        
        _alertLabel.SetActive(true);
        _evasionLabel.SetActive(false);
        //Debug.Log("ПОДНЯЛИ ТРЕВОГУ");
        _state = States.Alert;
        _timer = 0f;
    }
    public void SetEvasion()
    {
        _soundCheck = false;
        _alertLabel.SetActive(false);
        _evasionLabel.SetActive(true);
        //Debug.Log("ИЩЕМ ПРОТИВНИКА");
        _state = States.Search;
        isAlertTimer = false;
        isSearchTimer = true;

    }

    public  void SetNormal()
    {
        _evasionLabel.SetActive(false);
        //Debug.Log("всё спокойно");
        _state = States.Normal;
        isSearchTimer = false;
        isAlertTimer = false;
        _timer = 0f;
    }
}
