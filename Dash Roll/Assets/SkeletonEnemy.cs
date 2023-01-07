using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : Enemy
{
    [SerializeField] int speed, attackRange, leapRange, leapXPower, leapYPower;
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
        if (rb.velocity.y < -18) { SetYVelocity(-18); }

        if (timer > 0)
        {
            if (timer == 29) { hurtbox.Activate(IsFacingRight()); }
            if (timer == 27) { hurtbox.Deactivate(); }
            if (timer == 20) { timer -= Random.Range(10, 14); }
            timer--;
        }
        else if (InBoxDistanceToPlayer(trackingRange) && PlayerInSight())
        {
            FacePlayer();
            AddForwardXVelocity(speed, speed);
            animator.QueAnimation(animator.Walk, 10);

            if (Mathf.Abs(trfm.position.y - PlayerMovement.trfm.position.y) < 1)
            {
                if (Mathf.Abs(trfm.position.x - PlayerMovement.trfm.position.x) < attackRange)
                {
                    animator.QueAnimation(animator.Attack, 85);
                    timer = 43;
                }
                else
                {
                    if (Mathf.Abs(trfm.position.x - PlayerMovement.PredictedPosition(28).x) < leapRange + 1)
                    {
                        if (Mathf.Abs(trfm.position.x - PlayerMovement.PredictedPosition(28).x) > leapRange - 1)
                        {
                            AddForwardVelocity(leapXPower, leapYPower);
                            animator.QueAnimation(animator.Leap, 85);
                            timer = 47;
                        }
                    }
                }
            }
        }
    }
}
