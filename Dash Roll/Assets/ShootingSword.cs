using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSword : Projectile
{
    public static ObjectPooler slashFXPooler, hitRingFXPooler;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite embeddedSprite;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] ParticleSystem disintegrate, flight;
    [SerializeField] CapsuleCollider2D capsuleCollider;
    int timer; bool embedded, started;

    new void Start()
    {
        if (!started)
        {
            base.Start();
            trfm.Rotate(Vector3.forward * -90);
            started = true;
        }
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        if (timer == 1)
        {
            trailRenderer.emitting = true;
        }
        if (timer == 200)
        {
            spriteRenderer.enabled = false;
            disintegrate.Play();
        }
        if (timer == 350)
        {
            Destroy(gameObject);
        }
        timer++;

        if (!embedded)
        {
            base.FixedUpdate();
        }
    }

    new private void OnTriggerEnter2D(Collider2D col)
    {
        if (!started) { Start(); }
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            if (col.GetComponent<HPEntity>().TakeDamage(damage, entityType, trfm.up * knockback) != HPEntity.IGNORED)
            {
                slashFXPooler.Instantiate(col.transform.position, Random.Range(-45, 46));
                hitRingFXPooler.Instantiate(col.transform.position, 0);
                CameraController.SetTrauma(15);
            }
        }
        else if (col.gameObject.layer == 7)
        {
            TestTerrainContact();
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            TestTerrainContact();
        }
    }

    void TestTerrainContact()
    {
        if (timer > 0)
        {
            embedded = true;
            timer = 199;
            spriteRenderer.sprite = embeddedSprite;
            capsuleCollider.enabled = false;
            flight.Stop();
            trailRenderer.emitting = false;
        }
    }
}
