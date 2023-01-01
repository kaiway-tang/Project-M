using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickAttack : DirectionalAttack
{
    [SerializeField] PlayerMovement playerMovement;
    new void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {
            if (col.GetComponent<MobileEntity>().TakeDamage(damage, entityType, knockbackDirections[direction]) == HPEntity.ALIVE)
            {
                col.GetComponent<Enemy>().kicked = 35;
                playerMovement.TakeKnockback(knockbackDirections[direction] * -.8f);
            }
        } else
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            if (col.GetComponent<MobileEntity>().TakeDamage(damage, entityType, knockbackDirections[direction]) == HPEntity.ALIVE)
            {
                playerMovement.TakeKnockback(knockbackDirections[direction] * -.8f);
            }
        }
    }
}
