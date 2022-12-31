using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickAttack : DirectionalAttack
{
    [SerializeField] PlayerMovement playerMovement;
    new void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.GetComponent<MobileEntity>().TakeDamage(damage, entityType, knockbackDirections[direction]))
        {
            playerMovement.TakeKnockback(knockbackDirections[direction] * -.8f);
        }
    }
}
