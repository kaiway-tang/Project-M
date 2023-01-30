using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] Transform trfm;
    [SerializeField] Vector3 rate;
    bool rising;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rising)
        {
            trfm.position += rate;
        }
    }

    public void Rise()
    {

    }
}
