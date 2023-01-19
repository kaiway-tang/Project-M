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
        if (rb.velocity.y < -18) { SetYVelocity(-18); }

        if (timer > 0)
        {
            if (timer < 25)
            {
                FacePlayer();
            }
            else
            {
                if (timer == 32)
                {
                    attackTrailFX.emitting = true;
                    hurtbox.Activate(IsFacingRight());
                    AddForwardXVelocity(45, 45);
                }
                if (timer == 27)
                {
                    spriteRenderer.sprite = neutralSprite;
                    attackTrailFX.emitting = false;
                    hurtbox.Deactivate();
                    SetXVelocity(0);
                    timer -= Random.Range(0,5);
                }
            }

            timer--;
        }
        else if (InBoxDistanceToPlayer(trackingRange) && PlayerInSight())
        {
            FacePlayer();

            if (Mathf.Abs(Player.trfm.position.x - trfm.position.x) < 8 && Mathf.Abs(Player.trfm.position.y - trfm.position.y + .3f) < .2f)
            {
                telegraphPooler.Instantiate(trfm.position);
                spriteRenderer.sprite = attackSprite;
                timer = 44;
            }
            else
            {
                AddForwardVelocity(forwardPower, jumpPower);
                timer = Random.Range(24, 27);
            }
        }
    }
}
