using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugEnemy : Enemy
{
    [SerializeField] int spitRange;
    [SerializeField] SimpleAnimation crawlAnimation;
    [SerializeField] Sprite windup, lunge;
    [SerializeField] GameObject spitball;

    [SerializeField] Transform firepoint;
    [SerializeField] DirectionalAttack attack;

    int selectedAttack; const int NONE = 0, LUNGE = 1, SPIT = 2;
    int timer, attackCooldown;
    
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
        if (InBoxDistanceToPlayer(trackingRange))
        {
            FacePlayer();

            if (rb.velocity.y < -18) { SetYVelocity(-18); }

            if (timer < 1)
            {
                if (attackCooldown < 1 && InBoxDistanceToPlayer(spitRange))
                {
                    if (Mathf.Abs(PlayerMovement.trfm.position.x - trfm.position.x) < 8 && Mathf.Abs(trfm.position.y - PlayerMovement.trfm.position.y - 1) < 2)
                    {
                        selectedAttack = LUNGE;
                        timer = 25;
                        PrepareAttack();
                        attackCooldown = Random.Range(10, 15);
                    }
                    else
                    {
                        selectedAttack = SPIT;
                        PrepareAttack();
                        attackCooldown = Random.Range(40,60);
                        timer = 20;
                    }
                }
                else
                {
                    timer = Random.Range(8,10);
                    crawlAnimation.Play();
                    AddForwardXVelocity(8,8);
                }
            }
            else
            {
                if (selectedAttack == LUNGE)
                {
                    if (timer == 12)
                    {
                        attack.Activate();
                        spriteRenderer.sprite = lunge;
                        AddForwardVelocity(20, 30);
                    }
                    if (timer == 1)
                    {
                        attack.Deactivate();
                        selectedAttack = NONE;
                    }
                }
                else if (selectedAttack == SPIT)
                {
                    if (timer == 7)
                    {
                        spriteRenderer.sprite = lunge;
                        Instantiate(spitball, firepoint.position, Toolbox.GetQuaternionToPlayer(firepoint.position));
                        AddForwardXVelocity(-16, -16);
                        selectedAttack = NONE;
                    }
                }
            }

            timer--;

            if (attackCooldown > 0) { attackCooldown--; }
        }
    }

    void PrepareAttack()
    {
        spriteRenderer.sprite = windup;
        telegraphPooler.Instantiate(reflectionTrfm.position + reflectionTrfm.right);
    }
}
