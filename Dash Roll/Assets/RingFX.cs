using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingFX : PooledObject
{
    [SerializeField] Vector3 startScale = new Vector3(.4f, .4f, 1);
    [SerializeField] float scaleRate;
    // Start is called before the first frame update
    new void OnEnable()
    {
        base.OnEnable();
        trfm.localScale = startScale;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();

        trfm.localScale *= scaleRate;
    }
}
