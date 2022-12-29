using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Attack
{
    [SerializeField] int knockback;
    [SerializeField] Vector2 vect2;
    // Start is called before the first frame update
    new void OnTriggerEnter2D(Collider2D col)
    {
        vect2.x = trfm.right.x * knockback;
        vect2.y = .5f;
        col.GetComponent<MobileEntity>().TakeDamage(damage, entityType, vect2);
    }
}
