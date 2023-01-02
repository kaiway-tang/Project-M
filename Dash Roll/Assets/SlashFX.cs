using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashFX : PooledObject
{
    [SerializeField] Vector3 initialScale, changeScale;
    // Start is called before the first frame update

    private new void OnEnable()
    {
        base.OnEnable();
        trfm.localScale = initialScale;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        trfm.localScale += changeScale;
    }
}
