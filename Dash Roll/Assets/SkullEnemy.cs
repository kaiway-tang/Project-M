using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullEnemy : Enemy
{
    [SerializeField] int jumpPower, forwardPower;
    [SerializeField] Sprite neutralSprite, attackSprite;
    [SerializeField] TrailRenderer attackTrailFX;
    [SerializeField] DirectionalAttack hurtbox;
    int timer;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (everyTwo) { EveryTwo(); }
    }
    void EveryTwo()
    {
        if (InBoxDistanceToPlayer(trackingRange) && PlayerInSight())
        {
            if (rb.velocity.y < -18) { SetYVelocity(-18); }

            if (timer > 0)
            {
                if (timer < 25)
                {
                    FacePlayer();
                }
                else
                {
                    if (timer == 31)
                    {
                        attackTrailFX.emitting = true;
                        hurtbox.Activate(IsFacingRight());
                        AddForwardXVelocity(35, 35);
                    }
                    if (timer == 26)
                    {
                        spriteRenderer.sprite = neutralSprite;
                        attackTrailFX.emitting = false;
                        hurtbox.Deactivate();
                        SetXVelocity(0);
                    }
                }

                timer--;
            }
            else
            {
                if (Mathf.Abs(PlayerMovement.trfm.position.x - trfm.position.x) < 6 && Mathf.Abs(PlayerMovement.trfm.position.y - trfm.position.y - .2f) < .2f)
                {
                    telegraphPooler.Instantiate(trfm.position);
                    spriteRenderer.sprite = attackSprite;
                    timer = 44;
                }
                else
                {
                    AddForwardVelocity(forwardPower, jumpPower);
                    timer = 25;
                }
            }
        }
    }
}
