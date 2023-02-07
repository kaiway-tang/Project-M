using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountTrigger : MonoBehaviour
{
    [SerializeField] bool dismount;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 1)
        {
            if (dismount)
            {
                Player.trfm.parent = null;
            }
            else
            {
                Player.trfm.parent = ElevatorManagerIII.trfm;
            }
        } else if (col.gameObject.layer == 12)
        {
            if (dismount)
            {
                col.transform.parent = null;
            }
            else
            {
                col.transform.parent = ElevatorManagerIII.trfm;
            }
        } else if (col.gameObject.layer == 8)
        {
            try
            {
                col.gameObject.GetComponent<ShootingSword>().OnElevator(!dismount);
            }
            catch
            {

            }
        }
    }
}
