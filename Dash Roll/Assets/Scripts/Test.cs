using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] GameObject firebolt;
    [SerializeField] Transform trfm;
    int time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.rotation = Toolbox.GetQuaternionToPlayerPredicted(trfm.position, 18);
        if (time > 0) { time--; }
        else
        {
            time = 25;
            Instantiate(firebolt, trfm.position, trfm.rotation);
        }
    }
}
