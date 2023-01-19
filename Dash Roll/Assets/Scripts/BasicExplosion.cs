using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicExplosion : Attack
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color darken, fade;
 
    int timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer++;
        if (timer < 10)
        {
            spriteRenderer.color -= darken;
        }
        if (timer == 5) { Deactivate(); }
        if (timer > 5)
        {
            spriteRenderer.color -= fade;
        }
        if (timer > 25)
        {
            Destroy(gameObject);
        }
    }

    private new void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14) { col.GetComponent<HPEntity>().TakeDamage(damage, entityType, trfm.position, 25); }
    }
}
