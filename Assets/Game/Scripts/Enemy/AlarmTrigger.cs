using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{
    public bool setAlarm = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider coll)
    {
        if (setAlarm)
        {
            setAlarm = false;

            if (coll.gameObject.tag == "Enemy")
            {
                coll.GetComponent<Enemy>().SetStateToInvestigating();
                Debug.Log(coll.name);
            }
        }
        
    }


}
