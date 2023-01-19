using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttack : SlashAttack
{
    [SerializeField] Player playerScript;
    private new void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        if (takeDamageResult != HPEntity.IGNORED)
        {
            playerScript.hover = 12;
            if (!playerScript.IsTouchingGround()) { playerScript.TakeKnockback(new Vector2(0, 17)); }
        }
    }
}
