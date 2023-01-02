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

        if (tookDamage)
        {
            FlashWhite();
            tookDamage = false;
        }

        if (InBoxDistanceToPlayer(trackingRange))
        {
            if (rb.velocity.y < -15) { SetYVelocity(-15); }

            if (timer > 0)
            {
                if (timer < 50)
                {
                    FacePlayer();
                }
                else
                {
                    if (timer == 61) 
                    {
                        attackTrailFX.emitting = true;
                        hurtbox.Activate(IsFacingRight());
                        AddForwardXVelocity(35, 35);
                    }
                    if (timer == 51)
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
                    timer = 86;
                }
                else
                {
                    AddForwardVelocity(forwardPower, jumpPower);
                    timer = 50;
                }
            }
        }
    }
}
