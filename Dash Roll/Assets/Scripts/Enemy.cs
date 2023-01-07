using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MobileEntity
{
    [SerializeField] protected float friction;
    [SerializeField] protected int trackingRange;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [SerializeReference] GameObject deathFX;

    public static ObjectPooler telegraphPooler, kickRingFXPooler;
    public static Material defaultMaterial, flashMaterial;
    int hurtTimer;
    public int kicked;

    static int terrainLayerMask = 1 << 7; //layer mask to only test for terrain collisions
    protected bool everyTwo;

    protected new void Start()
    {
        base.Start();
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
            if (kicked == 51)
            {
                kickRingFXPooler.Instantiate(trfm.position, 90);
            }
            kicked--;
        }

        everyTwo = !everyTwo;
    }

    protected void Die()
    {
        Instantiate(deathFX, trfm.position, Quaternion.identity);
        End();
    }

    protected void FacePlayer()
    {
        if (PlayerMovement.trfm.position.x - trfm.position.x > 0) { FaceRight(); }
        else { FaceLeft(); }
    }
    protected void RotateToPlayer()
    {
        trfm.up = PlayerMovement.trfm.position - trfm.position;
    }
    public bool InBoxDistanceToPlayer(float distance)
    {
        return Mathf.Abs(trfm.position.x - PlayerMovement.trfm.position.x) < distance && Mathf.Abs(trfm.position.y - PlayerMovement.trfm.position.y) < distance;
    }

    protected void FlashWhite()
    {
        spriteRenderer.material = flashMaterial;
        hurtTimer = 8;
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (kicked > 20 && kicked < 49 && col.gameObject.layer == 12)
        {
            PlayerMovement.AddMana(10, col.GetComponent<HPEntity>().GetHP());
            TakeDamage(10, HPEntity.EntityTypes.Player);
            if (col.GetComponent<Enemy>().KickChained(rb.velocity) == HPEntity.ALIVE)
            {
                rb.velocity = Vector3.zero;
            }
            kicked = 20;
        }
    }

    int KickChained(Vector2 velocity)
    {
        if (kicked > 0) { return HPEntity.IGNORED; }
        kicked = 52;
        return TakeDamage(10, HPEntity.EntityTypes.Player, velocity);
    }

    protected bool PlayerInSight()
    {
        return !Physics2D.Linecast(trfm.position, PlayerMovement.trfm.position, terrainLayerMask);
    }

    protected bool ObstructedSightLine(Vector2 start, Vector2 end)
    {
        return Physics2D.Linecast(start, end, terrainLayerMask);
    }
}
