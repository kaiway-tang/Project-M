using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerEnemy : Enemy
{
    [SerializeField] GameObject fireball, firebolts, meteor, skull;
    [SerializeField] int attackTimer, castTime, attack, attackRange, teleportCooldown, laserCooldown;
    [SerializeField] float speed;
    [SerializeField] NecromancerAnimator animator;
    [SerializeReference] Transform firepoint;
    [SerializeField] ParticleSystem teleportIn, teleportOut, teleportReady;
    [SerializeField] Laser laser;

    Transform laserTrfm;
    static Vector3 meteorOffset;
    Vector2 teleportDestination;
    bool inRangeAndSight, initiated;
    
    new void Start()
    {
        base.Start();
        meteorOffset.y = 20;
        teleportCooldown = 140;

        laserTrfm = laser.transform;
        laserTrfm.parent = null;
        laserCooldown = 300;
    }


    bool everyFour;
    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (everyTwo) { EveryTwo(); }

        if (!initiated) { return; }

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

                attackTimer = Random.Range(50, 110);
            }

            attackTimer--;

            if (laserCooldown > 0)
            {
                if (laserCooldown < 72)
                {
                    if (laserCooldown % 35 == 1)
                    {
                        laserTrfm.position = Player.PredictedPosition(20);
                        laser.Fire();
                    }
                }

                laserCooldown--;
            }
            else
            {
                laserCooldown = Random.Range(250,350);
            }
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
                    Instantiate(fireball, firepoint.position, Toolbox.GetQuaternionToPlayerPredicted(firepoint.position, (int)Vector2.Distance(trfm.position, Player.trfm.position)));
                }
                else if (attack == 1) //triple bolt
                {
                    Instantiate(firebolts, firepoint.position, Toolbox.GetQuaternionToPlayerPredicted(firepoint.position, (int)Vector2.Distance(trfm.position, Player.trfm.position)));
                }
                else if (attack == 2) //meteor
                {
                    if (IsFacingLeft()) { meteorOffset.x = 6; }
                    else { meteorOffset.x = -6; }
                    Instantiate(meteor, Player.trfm.position + meteorOffset, Quaternion.identity);
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
                teleportCooldown -= Random.Range(0,50);
            }
            if (teleportCooldown == 1)
            {
                teleportReady.Play();
            }
            teleportCooldown--;
        }
        else if (InBoxDistanceToPlayer(6))
        {
            if (IsFacingRight())
            {
                Teleport(trfm.position + Vector3.right * 16);
            }
            else
            {
                Teleport(trfm.position + Vector3.right * -16);
            }
        }
    }
    void EveryTwo()
    {
        everyFour = !everyFour;
        if (everyFour) { EveryFour(); }

        if (!initiated) { return; }

        FacePlayer();
        ApplyYFriction(friction * 2);

        if (attackTimer > 0) { Kite(); }
    }

    void EveryFour()
    {
        if (PlayerInSight())
        {
            inRangeAndSight = InBoxDistanceToPlayer(attackRange);

            if (!initiated && inRangeAndSight) { initiated = true; }
        }
        else if (initiated)
        {
            inRangeAndSight = false;

            if (teleportCooldown == 0)
            {
                vect3.x = (Random.Range(0, 2) * 2 - 1) * 7;
                vect3.y = 6;
                vect3.z = 0;
                if (!ObstructedSightLine(Player.trfm.position + vect3, Player.trfm.position))
                {
                    Teleport(Player.trfm.position + vect3);
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

        if (trfm.position.y < Player.trfm.position.y + 5)
        {
            AddYVelocity(speed * 1.5f, speed * 5);
            still = false;
        }
        else if (trfm.position.y > Player.trfm.position.y + 7)
        {
            AddYVelocity(-speed * 1.5f, -speed * 5);
            still = false;
        }

        if (Mathf.Abs(trfm.position.x - Player.trfm.position.x) < 6)
        {
            AddForwardXVelocity(-speed, -speed * 5);
            still = false;
        }
        else if (Mathf.Abs(trfm.position.x - Player.trfm.position.x) > 10)
        {
            AddForwardXVelocity(speed, speed * 5);
            still = false;
        }

        if (still) { animator.RequestAnimatorState(animator.Idle); }
        else { animator.RequestAnimatorState(animator.Fly); }
    }
}
