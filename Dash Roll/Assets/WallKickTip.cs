using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallKickTip : TutorialTip
{
    [SerializeField] SpriteRenderer SpriteRenderer;

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            SpriteRenderer.enabled = true;
            PlayerMovement.playerMovement.hover = 999;
            EnterSloMo(.5f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            SpriteRenderer.enabled = false;
            PlayerMovement.playerMovement.hover = 1;
            ExitSloMo();
        }
    }
}
