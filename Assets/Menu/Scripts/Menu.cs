using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private GameObject _menuButtons;
    [SerializeField] private GameObject _startButton;
    
    private bool _canPlayCrossFade;
    private bool _enterButton;
    public void Start()
    {
        _enterButton = true;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void Update()
    {
        if (_enterButton)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartButton();
            }
        }
        
    }

    public void StartButton()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _startButton.gameObject.SetActive(false);
        _menuButtons.gameObject.SetActive(true);


    }
    
    public void Game()
    {
        SceneManager.LoadScene(2);
       
    }
   


    public void Exit()
    {
        Application.Quit();
    }
   /* void Update()
    {
        if (_canPlayCrossFade)
        {
            animator.Play("CrossFade_End");
            _canPlayCrossFade = false;
        }

    }*/

}
