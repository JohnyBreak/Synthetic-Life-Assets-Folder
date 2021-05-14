using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHelper : MonoBehaviour
{
    [SerializeField] private GameObject _controlHelper;
    private bool _showHelper = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _showHelper = !_showHelper;
            _controlHelper.SetActive(_showHelper);
        }
       
    }
}
