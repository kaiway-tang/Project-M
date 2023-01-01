using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttack : DirectionalAttack
{
    [SerializeField] PlayerMovement playerMovement;
    Vector2 hover;
    private new void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            if (col.GetComponent<HPEntity>().TakeDamage(damage, entityType, knockbackDirections[direction]) != HPEntity.IGNORED)
            {
                playerMovement.hover = 12;
            }
        }
    }
}
