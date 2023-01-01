using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MobileEntity
{
    [SerializeField] protected float friction;
    [SerializeField] protected int trackingRange;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    public static ObjectPooler telegraphPooler;
    public static Material defaultMaterial, flashMaterial;
    int hurtTimer;
    public int kicked;

    protected void Start()
    {

    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
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
            kicked--;
        }
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
        hurtTimer = 5;
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (kicked > 25 && col.gameObject.layer == 12)
        {
            col.gameObject.GetComponent<Enemy>().KickChained(rb.velocity);
        }
    }

    void KickChained(Vector2 velocity)
    {
        if (kicked > 0) { return; }
        TakeDamage(10, HPEntity.EntityTypes.Player, velocity);
        kicked = 35;
    }
}
