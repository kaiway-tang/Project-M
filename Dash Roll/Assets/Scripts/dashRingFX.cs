using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashRingFX : PooledObject
{
    [SerializeField] Vector3 change;
    new void OnEnable()
    {
        base.OnEnable();
        trfm.localScale = new Vector3(2, 2, 1);
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        trfm.localScale += change;
    }
}
