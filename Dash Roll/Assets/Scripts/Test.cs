using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] int layerMask = 1 << 7;
    [SerializeField] bool playerVisible;

    bool everyTwo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerVisible = !Physics2D.Linecast(transform.position, PlayerMovement.trfm.position, layerMask);
    }
}
