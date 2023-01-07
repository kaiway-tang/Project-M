using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kickVFX : SelfDest
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color fade;
    [SerializeField] Transform trfm;
    [SerializeField] float speed;
    int timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        if (timer > 2)
        {
            spriteRenderer.color -= fade;
        }
        base.FixedUpdate();
        trfm.position += trfm.right * speed;
        timer++;
    }
}
