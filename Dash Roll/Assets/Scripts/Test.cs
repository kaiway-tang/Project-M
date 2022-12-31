using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Transform trfm, playerTrfm;

    bool everyTwo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.right = playerTrfm.position - trfm.position;
    }

    void EveryTwo()
    {
        
    }
}
