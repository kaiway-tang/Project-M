using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugEnemy : Enemy
{
    [SerializeField] int spitRange, lungeRange, speed, lungePower;
    [SerializeField] int[] cooldownLengths;
    [SerializeField] SimpleAnimation crawlAnimation;
    [SerializeField] Sprite windup, lunge;
    [SerializeField] GameObject spitball;
    [SerializeField] TrailRenderer attackTrailFX;

    [SerializeField] Transform firepoint;
    [SerializeField] DirectionalAttack attack;

    int selectedAttack, timer, attackCooldown;
    const int NONE = 0, LUNGE = 1, SPIT = 2;

    Quaternion spitBallAim;
    
    new void Start()
    {
        timer = Random.Range(10, 14);
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
        if (InBoxDistanceToPlayer(trackingRange) && PlayerInSight() && timer < 1)
        {
            if (attackCooldown < 1 && InBoxDistanceToPlayer(spitRange))
            {
                if (Mathf.Abs(Player.trfm.position.x - trfm.position.x) < lungeRange && Mathf.Abs(trfm.position.y - Player.trfm.position.y - 1) < 2)
                {
                    selectedAttack = LUNGE;
                    timer = 20;
                    PrepareAttack();
                    attackCooldown = Random.Range(cooldownLengths[0], cooldownLengths[1]); //10, 16
                }
                else
                {
                    selectedAttack = SPIT;
                    PrepareAttack();
                    attackCooldown = Random.Range(cooldownLengths[0] * 2, cooldownLengths[1] * 2); //30, 41
                    timer = 15;
                }
            }
            else
            {
                timer = Random.Range(11, 14);
                crawlAnimation.Play();
            }
            FacePlayer();
        }
        else
        {
            if (selectedAttack == LUNGE)
            {
                if (timer == 12)
                {
                    attack.Activate();
                    attackTrailFX.emitting = true;
                    spriteRenderer.sprite = lunge;
                    AddForwardVelocity(lungePower, 30);
                }
                if (timer == 1)
                {
                    attack.Deactivate();
                    attackTrailFX.emitting = false;
                    selectedAttack = NONE;
                }
            }
            else if (selectedAttack == SPIT)
            {
                if (timer == 11)
                {
                    spitBallAim = Toolbox.GetQuaternionToPlayerHead(firepoint.position);
                    FacePlayer();
                }
                if (timer == 7)
                {
                    spriteRenderer.sprite = lunge;
                    Instantiate(spitball, firepoint.position, spitBallAim);
                    AddForwardXVelocity(-16, -16);
                }
                if (timer == 1)
                {
                    selectedAttack = NONE;
                }
            }
            else if (selectedAttack == NONE)
            {
                if (timer == 5)
                {
                    AddForwardXVelocity(speed, speed);
                }
            }

            if (rb.velocity.y < -18) { SetYVelocity(-18); }
            timer--;
        }

        if (attackCooldown > 0) { attackCooldown--; }
    }

    void PrepareAttack()
    {
        spriteRenderer.sprite = windup;
        telegraphPooler.Instantiate(reflectionTrfm.position + reflectionTrfm.right);
    }
}
