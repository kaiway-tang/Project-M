using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : Enemy
{
    [SerializeField] GameObject projectile;
    [SerializeField] Sprite loaded, fired;
    [SerializeField] Transform turretTrfm;
    [SerializeField] int attackCooldown, maxDecrement;
    [SerializeField] float predictionMultiplier;
    int attackTimer;

    bool every2;
    protected new void Start()
    {
        base.Start();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        if (every2) { EveryTwo(); }
        every2 = !every2;
    }

    void EveryTwo()
    {
        if (attackTimer > 0)
        {
            if (attackTimer == 40)
            {
                Instantiate(projectile, turretTrfm.position + turretTrfm.up * 2, turretTrfm.rotation);
                spriteRenderer.sprite = fired;
            }
            if (attackTimer == 20)
            {
                attackTimer -= Random.Range(0,maxDecrement);
            }
            if (attackTimer == 10)
            {
                spriteRenderer.sprite = loaded;
            }
            attackTimer--;
        }

        if (InBoxDistanceToPlayer(trackingRange) && PlayerInSight())
        {
            RotateTowardsPlayerPredicted(trfm, (int)(Vector2.Distance(trfm.position, Player.trfm.position) * predictionMultiplier), .1f);

            if (attackTimer < 1)
            {
                telegraphPooler.Instantiate(trfm.position + trfm.up);
                attackTimer = attackCooldown;
            }
        }
    }
}
