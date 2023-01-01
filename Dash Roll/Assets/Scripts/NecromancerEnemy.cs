using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerEnemy : Enemy
{
    [SerializeField] GameObject fireball, firebolts, meteor, skull;
    [SerializeField] int attackTimer, castTime, attack, attackRange, teleportCooldown;
    [SerializeField] float speed;
    [SerializeField] NecromancerAnimator animator;
    [SerializeReference] Transform firepoint;
    [SerializeField] TrailRenderer teleportTrail;

    Vector3 meteorOffset;
    bool inRange;
    
    new void Start()
    {
        base.Start();
        meteorOffset.y = 20;
    }


    bool everyTwo, everyFour;
    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (inRange)
        {
            if (attackTimer < 1) //at timer == 0, select next attack
            {
                attack = Random.Range(0,4);

                if (attack == 0) //explosive fireball
                {
                    animator.QueAnimation(animator.CastSpell, 60);
                    castTime = 40;
                }
                else if (attack == 1) //triple bolt
                {
                    animator.QueAnimation(animator.CastSpell, 60);
                    castTime = 40;
                }
                else if (attack == 2) //meteor
                {
                    animator.QueAnimation(animator.CastSpell, 60);
                    castTime = 40;
                }
                else if (attack == 3) //summon skull
                {
                    animator.QueAnimation(animator.Summon, 64);
                    castTime = 44;
                }

                attackTimer = Random.Range(70, 80);
            }

            if (teleportCooldown > 0)
            {
                if (teleportCooldown == 149) { teleportTrail.emitting = false; }
                teleportCooldown--;
            }
            else if (InBoxDistanceToPlayer(4))
            {
                teleportTrail.emitting = true;
                teleportCooldown = 150;
                vect3.y = 0; vect3.z = 0;
                if (IsFacingRight())
                {
                    vect3.x = 10;
                    trfm.position += vect3;
                }
                else
                {
                    vect3.x = -10;
                    trfm.position += vect3;
                }
            }

            attackTimer--;
        }

        if (castTime > 0)
        {
            if (castTime == 1)
            {
                if (attack == 0) //explosive fireball
                {
                    Instantiate(fireball, firepoint.position, Toolbox.GetQuaternionToPlayer(firepoint.position));
                }
                else if (attack == 1) //triple bolt
                {
                    Instantiate(firebolts, firepoint.position, Toolbox.GetQuaternionToPlayer(firepoint.position));
                }
                else if (attack == 2) //meteor
                {
                    if (IsFacingLeft()) { meteorOffset.x = 6; }
                    else { meteorOffset.x = -6; }
                    Instantiate(meteor, PlayerMovement.trfm.position + meteorOffset, Quaternion.identity);
                }
                else if (attack == 3) //summon skull
                {
                    Instantiate(skull, trfm.position, trfm.rotation);
                }
            }
            castTime--;
        }

        if (tookDamage)
        {
            FlashWhite();
            tookDamage = false;
        }

        everyTwo = !everyTwo;
        if (everyTwo) { EveryTwo(); }
    }
    void EveryTwo()
    {
        FacePlayer();
        ApplyXFriction(friction);
        ApplyYFriction(friction * 2);

        inRange = InBoxDistanceToPlayer(attackRange);

        if (attackTimer > 0) { Kite(); }

        everyFour = !everyFour;
        if (everyFour) { EveryFour(); }
    }

    void EveryFour()
    {
        
    }

    bool still;
    void Kite()
    {
        still = true;

        if (trfm.position.y < PlayerMovement.trfm.position.y + 5)
        {
            AddYVelocity(speed * 1.5f, speed * 2);
            still = false;
        }
        else if (trfm.position.y > PlayerMovement.trfm.position.y + 7)
        {
            AddYVelocity(-speed * 1.5f, -speed * 2);
            still = false;
        }

        if (Mathf.Abs(trfm.position.x - PlayerMovement.trfm.position.x) < 6)
        {
            AddForwardXVelocity(-speed, -speed * 2);
            still = false;
        }
        else if (Mathf.Abs(trfm.position.x - PlayerMovement.trfm.position.x) > 10)
        {
            AddForwardXVelocity(speed, speed * 2);
            still = false;
        }

        if (still) { animator.RequestAnimatorState(animator.Idle); }
        else { animator.RequestAnimatorState(animator.Fly); }
    }
}
