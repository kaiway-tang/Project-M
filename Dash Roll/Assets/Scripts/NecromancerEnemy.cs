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
    [SerializeField] ParticleSystem teleportIn, teleportOut, teleportReady;

    static Vector3 meteorOffset;
    Vector2 teleportDestination;
    bool inRangeAndSight;
    
    new void Start()
    {
        base.Start();
        meteorOffset.y = 20;
        teleportCooldown = 140;
    }


    bool everyFour;
    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (inRangeAndSight)
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

            attackTimer--;
        }

        if (castTime > 0)
        {
            if (castTime == 25)
            {
                telegraphPooler.Instantiate(trfm.position + Vector3.up * 3);
            }
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
                    Instantiate(skull, trfm.position + Toolbox.GetUnitVectorToPlayer(trfm.position), trfm.rotation);
                }
            }
            castTime--;
        }

        if (teleportCooldown > 0)
        {
            if (teleportCooldown == 145)
            {
                trfm.position = teleportDestination;
                teleportIn.Play();
            }
            if (teleportCooldown == 1)
            {
                teleportReady.Play();
            }
            teleportCooldown--;
        }
        else if (InBoxDistanceToPlayer(4))
        {
            if (IsFacingRight())
            {
                Teleport(trfm.position + Vector3.right * 10);
            }
            else
            {
                Teleport(trfm.position + Vector3.right * -10);
            }
        }

        if (everyTwo) { EveryTwo(); }
    }
    void EveryTwo()
    {
        FacePlayer();
        ApplyXFriction(friction);
        ApplyYFriction(friction * 2);

        if (attackTimer > 0) { Kite(); }

        everyFour = !everyFour;
        if (everyFour) { EveryFour(); }
    }

    void EveryFour()
    {
        if (PlayerInSight())
        {
            inRangeAndSight = InBoxDistanceToPlayer(attackRange);
        }
        else
        {
            inRangeAndSight = false;

            if (teleportCooldown == 0)
            {
                vect3.x = (Random.Range(0, 2) * 2 - 1) * 7;
                vect3.y = 6;
                vect3.z = 0;
                if (!ObstructedSightLine(PlayerMovement.trfm.position + vect3, PlayerMovement.trfm.position))
                {
                    Teleport(PlayerMovement.trfm.position + vect3);
                }
            }
        }
    }

    void Teleport(Vector2 destination)
    {
        teleportDestination = destination;
        teleportReady.Stop();
        teleportOut.Play();
        teleportCooldown = 150;
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
