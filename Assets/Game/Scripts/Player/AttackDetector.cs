using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetector : MonoBehaviour
{
    [SerializeField] private playerController playerCtrl;
    [SerializeField] private float _damage;
    public void OnTriggerEnter(Collider collider)
    {
        if (playerCtrl.isPunching)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<Target>().takeMeleeDamage(_damage);
                //Debug.Log("damage = " + _damage);
            }
        }
    }
}
