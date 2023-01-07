using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugEnemy : Enemy
{
    [SerializeField] int spitRange, speed;
    [SerializeField] SimpleAnimation crawlAnimation;
    [SerializeField] Sprite windup, lunge;
    [SerializeField] GameObject spitball;

    [SerializeField] Transform firepoint;
    [SerializeField] DirectionalAttack attack;

    int selectedAttack; const int NONE = 0, LUNGE = 1, SPIT = 2;
    int timer, attackCooldown;

    Quaternion spitBallAim;
    
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
        if (InBoxDistanceToPlayer(trackingRange) && PlayerInSight() && timer < 1)
        {
            if (attackCooldown < 1 && InBoxDistanceToPlayer(spitRange))
            {
                if (Mathf.Abs(PlayerMovement.trfm.position.x - trfm.position.x) < 8 && Mathf.Abs(trfm.position.y - PlayerMovement.trfm.position.y - 1) < 2)
                {
                    selectedAttack = LUNGE;
                    timer = 20;
                    PrepareAttack();
                    attackCooldown = Random.Range(10, 16);
                }
                else
                {
                    selectedAttack = SPIT;
                    PrepareAttack();
                    attackCooldown = Random.Range(60, 81);
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
                    spriteRenderer.sprite = lunge;
                    AddForwardVelocity(speed * 2, 30);
                }
                if (timer == 1)
                {
                    attack.Deactivate();
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
            if (attackCooldown > 0) { attackCooldown--; }
            timer--;
        }
    }

    void PrepareAttack()
    {
        spriteRenderer.sprite = windup;
        telegraphPooler.Instantiate(reflectionTrfm.position + reflectionTrfm.right);
    }
}
