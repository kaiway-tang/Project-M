using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MobileEntity
{
    [SerializeField] protected float friction;
    [SerializeField] protected int trackingRange, healthShards = 2;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] Gate[] triggeredGates;

    [SerializeReference] GameObject deathFX;
    [SerializeField] GameObject[] spawnObjects;
    [SerializeField] Vector2[] spawnObjectsVelocity;

    public static ObjectPooler telegraphPooler, kickRingFXPooler, bloodFXLargePooler, healthShardPooler;
    public static Material defaultMaterial, flashMaterial;
    int hurtTimer;
    public int kicked;
    float lastVelocity;

    static int terrainLayerMask = 1 << 7; //layer mask to only test for terrain collisions
    protected bool everyTwo;

    protected new void Start()
    {
        base.Start();
    }

    public void InitiateSpawnObject(Vector2 spawnVelocity, Gate[] pTriggeredGates)
    {
        rb.velocity = spawnVelocity;
        triggeredGates = pTriggeredGates;
    }
    public void InitiateSpawnObject(Vector2 spawnVelocity)
    {
        rb.velocity = spawnVelocity;
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        if (tookDamage)
        {
            if (HP <= 0) { Die(); }
            FlashWhite();
        }

        base.FixedUpdate();

        if (hurtTimer > 0)
        {
            if (hurtTimer == 1)
            {
                spriteRenderer.material = defaultMaterial;
            }
            hurtTimer--;
        }
        if (kicked > 0)
        {
            if (kicked > 10 && Mathf.Abs(rb.velocity.x) < 26)
            {
                if (Mathf.Abs(rb.velocity.x) < 1 && Mathf.Abs(rb.velocity.x - lastVelocity) > 26)
                {
                    TakeDamage(10, HPEntity.EntityTypes.Player);
                }
                kicked = 0;
            }
            lastVelocity = rb.velocity.x;

            kicked--;
        }

        if (everyTwo) { EveryTwo(); }
        everyTwo = !everyTwo;
    }

    void EveryTwo()
    {
        if (frictionToggle < 1) { ApplyXFriction(friction); }
    }

    protected void Die()
    {
        if (triggeredGates.Length > 0)
        {
            if (spawnObjects.Length > 0)
            {
                for (int i = 0; i < spawnObjects.Length; i++)
                {
                    Instantiate(spawnObjects[i], trfm.position, Quaternion.identity).GetComponent<Enemy>().InitiateSpawnObject(spawnObjectsVelocity[i], triggeredGates);
                } 
            }

            for (int i = 0; i < triggeredGates.Length; i++)
            {
                triggeredGates[i].IncrementDeathCount();
            }
        }
        else if (spawnObjects.Length > 0)
        {
            for (int i = 0; i < spawnObjects.Length; i++)
            {
                Instantiate(spawnObjects[i], trfm.position, Quaternion.identity).GetComponent<Enemy>().InitiateSpawnObject(spawnObjectsVelocity[i]);
            }
        }

        if (deathFX) { Instantiate(deathFX, trfm.position, Quaternion.identity); }
        else { bloodFXLargePooler.Instantiate(trfm.position); }

        for (int i = 0; i < healthShards; i++)
        {
            healthShardPooler.Instantiate(trfm.position);
        }

        End();
    }

    protected void FacePlayer()
    {
        if (Player.trfm.position.x - trfm.position.x > 0) { FaceRight(); }
        else { FaceLeft(); }
    }
    protected void RotateTowardsPlayer(Transform pTrfm, float rate = 1)
    {
        pTrfm.up = (Player.trfm.position - pTrfm.position) * rate;
    }
    protected void RotateTowardsPlayerPredicted(Transform pTrfm, int ticks, float rate = 1)
    {
        pTrfm.up = ((Vector3)Player.PredictedPosition(ticks) - pTrfm.position) * rate;
    }

    public bool InBoxDistanceToPlayer(float distance)
    {
        return Mathf.Abs(trfm.position.x - Player.trfm.position.x) < distance && Mathf.Abs(trfm.position.y - Player.trfm.position.y) < distance;
    }

    protected void FlashWhite()
    {
        spriteRenderer.material = flashMaterial;
        hurtTimer = 11;
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (kicked > 10 && kicked < 49 && col.gameObject.layer == 12)
        {
            if (col.GetComponent<Enemy>().KickChained(rb.velocity) == HPEntity.ALIVE)
            {
                rb.velocity = Vector3.zero;
            }
            TakeDamage(10, EntityTypes.Player);
            kicked = 10;
        }
    }

    int KickChained(Vector2 velocity)
    {
        if (kicked > 0) { return HPEntity.IGNORED; }

        Player.AddMana(10, HP);
        kickRingFXPooler.Instantiate(trfm.position, 90);
        kicked = 50;
        return TakeDamage(10, HPEntity.EntityTypes.Player, velocity);
    }

    protected bool PlayerInSight()
    {
        return !Physics2D.Linecast(trfm.position, Player.trfm.position, terrainLayerMask);
    }

    protected bool ObstructedSightLine(Vector2 start, Vector2 end)
    {
        return Physics2D.Linecast(start, end, terrainLayerMask);
    }

    protected const int ON = -1, OFF = 1;
    [SerializeField] int frictionToggle;
    protected void ToggleFriction(int state)
    {
        frictionToggle += state;
        if (frictionToggle < 0) { frictionToggle = 0; }
    }
}
