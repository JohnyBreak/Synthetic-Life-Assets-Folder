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
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _soundGO;
    [SerializeField] private GameObject _text;
    private AudioSource _cutSceneSound;
    [SerializeField] private int index;
    // Start is called before the first frame update
    void Start()
    {
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
        inCutScene = false;
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
    }

    public void PlayElevatorCutScene()
    {
        index = 1;
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
