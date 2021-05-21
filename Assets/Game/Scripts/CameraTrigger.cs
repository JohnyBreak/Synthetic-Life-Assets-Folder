using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private GameObject _trigger;
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.GetComponent<playerController>() != null)
        {
            _playableDirector.Play();
            _trigger.SetActive(false);
        }
    }
}
