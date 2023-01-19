using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpRingFX : PooledObject
{
    [SerializeField] Vector3 scale, initialScale;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color fade;
    // Start is called before the first frame update
    new void OnEnable()
    {
        base.OnEnable();
        trfm.localScale = initialScale;
        spriteRenderer.color = Color.white;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        trfm.localScale += scale;
        spriteRenderer.color -= fade;
    }
}
