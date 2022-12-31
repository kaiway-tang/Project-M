using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : SelfDest
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color fade;
    [SerializeField] Transform trfm;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();
        spriteRenderer.color -= fade;
        trfm.position += trfm.right * speed;
    }
}
