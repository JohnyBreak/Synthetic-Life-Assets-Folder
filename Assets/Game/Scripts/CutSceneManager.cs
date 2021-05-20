using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private List<PlayableDirector> _playableDirector;
    public bool inCutScene;
    private bool _isPlayingCutScene = false;
    [SerializeField] private GameObject _weapon;
    private WeaponSwitch _weaponHolder;
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _soundGO;
    [SerializeField] private GameObject _text;
    private AudioSource _cutSceneSound;
    [SerializeField] private int index;
    [SerializeField] private GameObject _player;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private RuntimeAnimatorController _playerRunTimeAnimCtrl;
    bool fix = false;
    void OnEnable()
    {
        
        _playerRunTimeAnimCtrl = _playerAnimator.runtimeAnimatorController;
        _playerAnimator.runtimeAnimatorController = null;
    }
    void Start()
    {
        _weaponHolder = _weapon.GetComponent<WeaponSwitch>();
        //_playerAnimator = _player.GetComponent<Animator>();
        _cutSceneSound = _soundGO.GetComponent<AudioSource>();
    }
    public void StartCutScene()
    {
        _hud.SetActive(false);
        _weapon.SetActive(false);
        inCutScene = true;
        _playableDirector[index].Play();
        //Debug.Log("biba");
        _isPlayingCutScene = true;
    }

    public void StopCutScene()
    {
        
        _hud.SetActive(true);
        _weapon.SetActive(true);
        _playableDirector[index].Stop();
        _isPlayingCutScene = false;
        inCutScene = false;
        ResetAnimator();
        //_playerAnimator.enabled = true;
        /*_playableDirector[index].time = _playableDirector[index].playableAsset.duration; // set the time to the last frame
        _playableDirector[index].Evaluate(); // evaluates the timeline
        _cutSceneSound.SetActive(false);
        _playableDirector[index].Stop(); // deletes the instance of the timeline

        Debug.Log("booooba");
        _isPlayingCutScene = false;*/
    }
    public void ChangeCutSceneState()
    {
        
        inCutScene = !inCutScene;
    }
    void ResetAnimator()
    {
        _playerAnimator.runtimeAnimatorController = _playerRunTimeAnimCtrl;
        //Debug.Log("ghbdtn");
        _weaponHolder.anim = _playerAnimator;
    }
    public void SkipCutScene()
    {
        
        _hud.SetActive(true);
        _weapon.SetActive(true);
        ChangeCutSceneState();
        _playableDirector[index].time = _playableDirector[index].playableAsset.duration; // set the time to the last frame
        _playableDirector[index].Evaluate(); // evaluates the timeline
        _soundGO.SetActive(false);
        _playableDirector[index].Stop(); // deletes the instance of the timeline
        
        //Debug.Log("booooba");
        _isPlayingCutScene = false;
        //ResetAnimator();
    }

    public void PlayElevatorCutScene()
    {
        index = 1;
        StartCutScene();
    }
    public void PlayJuliaCutScene()
    {
        index = 0;
        StartCutScene();
    }

    public void PauseCutScene()
    {
        _playableDirector[index].Pause();
        _cutSceneSound.Pause();
        _text.SetActive(false);
    }
    public void ResumeCutScene()
    {
        _text.SetActive(true);
        _playableDirector[index].Resume();
        _cutSceneSound.UnPause();
    }

    void Update()
    {
       /* if (_playableDirector[index].state != PlayState.Playing && !fix)
        {
            fix = true;
            _playerAnimator.runtimeAnimatorController = _playerRunTimeAnimCtrl;
            Debug.Log("ghbdtn");
        }*/
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (_isPlayingCutScene)
            {
                SkipCutScene();
            }
        }
        /*if (Input.GetKeyDown(KeyCode.P))
        {
           
            if (!_isPlayingCutScene)
            {
                StartCutScene();
            }
            else
            {
                SkipCutScene();
            }
        }*/
    }
}
