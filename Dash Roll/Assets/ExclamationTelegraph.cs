using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationTelegraph : PooledObject
{
    [SerializeField] Vector3 rise;
    [SerializeField] Transform trfm;
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        trfm.position += rise;
    }
}
