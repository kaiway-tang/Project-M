using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : Enemy
{
    [SerializeField] int speed, attackRange, leapRange, leapXPower, leapYPower;
    [SerializeField] float heightOffset;
    [SerializeField] DirectionalAttack hurtbox;
    [SerializeField] SkeletonAnimator animator;
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
        if (rb.velocity.y < -22) { SetYVelocity(-22); }

        if (timer > 0)
        {
            if (timer == 49) { hurtbox.Activate(IsFacingRight()); }
            if (timer == 47) { hurtbox.Deactivate(); ToggleFriction(ON); }
            if (timer == 40) { timer -= Random.Range(15, 21); }
            timer--;
        }
        else if (InBoxDistanceToPlayer(trackingRange) && PlayerInSight())
        {
            FacePlayer();
            AddForwardXVelocity(speed, speed);
            animator.QueAnimation(animator.Walk, 10);

            if (Mathf.Abs(trfm.position.y - Player.trfm.position.y - heightOffset) < 1)
            {
                if (Mathf.Abs(trfm.position.x - Player.trfm.position.x) < attackRange)
                {
                    animator.QueAnimation(animator.Attack, 85);
                    timer = 63;
                    telegraphPooler.Instantiate(trfm.position + Vector3.up * 2);
                }
                else
                {
                    if (Mathf.Abs(Mathf.Abs(trfm.position.x - Player.PredictedPosition(28).x) - leapRange) < 1)
                    {
                        ToggleFriction(OFF);
                        AddForwardVelocity(leapXPower, leapYPower);
                        animator.QueAnimation(animator.Leap, 85);
                        timer = 67;
                        telegraphPooler.Instantiate(trfm.position + Vector3.up * 2);
                    }
                }
            }
        }
    }
}
