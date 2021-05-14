using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{
    [SerializeField] private float _time;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(PlayIntro());
        
    }

    // Update is called once per frame
    IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(_time);
        SceneManager.LoadScene(1);
    }
}
