using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallKickTip : TutorialAction
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite alternateSprite;
    [SerializeField] Transform textTrfm;

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            if (TutorialManager.usingArrows && alternateSprite) { spriteRenderer.sprite = alternateSprite; }
            textTrfm.position = new Vector3(textTrfm.position.x, Player.trfm.position.y + 4, 0);
            spriteRenderer.enabled = true;
            Player.playerScript.hover = 999;
            EnterSloMo(.5f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            spriteRenderer.enabled = false;
            Player.playerScript.hover = 1;
            ExitSloMo();
        }
    }
}
