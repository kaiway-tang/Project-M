using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : Enemy
{
    [SerializeField] GameObject projectile;
    [SerializeField] int speed, range, shootingRange, timer;
    bool lockedOn;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new private void FixedUpdate()
    {
        base.FixedUpdate();

        if (everyTwo) { EveryTwo(); }
    }

    void EveryTwo()
    {
        if (timer > 0)
        {
            if (timer == 40)
            {
                Instantiate(projectile, trfm.position + Toolbox.GetUnitVectorToPlayer(trfm.position), Toolbox.GetQuaternionToPlayerPredicted(trfm.position, 15));
                timer -= Random.Range(0,10);
            }
            timer--;
        }

        if (lockedOn)
        {
            if (timer < 1 && InBoxDistanceToPlayer(shootingRange))
            {
                timer = 50;
                telegraphPooler.Instantiate(trfm.position);
            }

            Kite();

            if (!PlayerInSight() || !InBoxDistanceToPlayer(range * 2))
            {
                lockedOn = false;
            }
        }
        else
        {
            ApplyYFriction(friction);

            if (PlayerInSight() && InBoxDistanceToPlayer(range))
            {
                lockedOn = true;
            }
        }
    }

    void Kite()
    {
        FacePlayer();

        if (trfm.position.y < Player.trfm.position.y + 4)
        {
            AddYVelocity(speed * .7f, speed * 1.4f);
        }
        else if (trfm.position.y > Player.trfm.position.y + 6)
        {
            AddYVelocity(-speed, -speed * 2);
        }

        if (Mathf.Abs(trfm.position.x - Player.trfm.position.x) < 5)
        {
            AddForwardXVelocity(-speed, -speed * 2);
        }
        else if (Mathf.Abs(trfm.position.x - Player.trfm.position.x) > 10)
        {
            AddForwardXVelocity(speed, speed * 2);
        }
    }
}
