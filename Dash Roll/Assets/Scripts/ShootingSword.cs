using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSword : Projectile
{
    public static ObjectPooler slashFXPooler, hitRingFXPooler;
    bool started, embedded;

    new void Start()
    {
        if (!started)
        {
            base.Start();
            trfm.Rotate(Vector3.forward * -90);
            started = true;
        }
    }

    new void FixedUpdate()
    {
        if (!embedded)
        {
            base.FixedUpdate();
        }
    }

    public void Embed()
    {
        embedded = true;
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
    }
}
