using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttack : SlashAttack
{
    [SerializeField] PlayerMovement playerMovement;
    private new void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        if (takeDamageResult != HPEntity.IGNORED)
        {
            playerMovement.hover = 12;
            if (!playerMovement.IsTouchingGround()) { playerMovement.TakeKnockback(new Vector2(0, 17)); }
        }
    }
}
