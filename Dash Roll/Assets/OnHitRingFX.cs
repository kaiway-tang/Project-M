using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitRingFX : PooledObject
{
    [SerializeField] Vector2 expandScale;
    [SerializeField] Vector3 initialScale;
    [SerializeField] SimpleAnimation animator;
    // Start is called before the first frame update
    new void OnEnable()
    {
        base.OnEnable();

        trfm.localScale = initialScale;
        animator.Play();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        trfm.localScale *= expandScale;
    }
}
