using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShard : PooledObject
{
    [SerializeField] float spd, turnSpd;
    bool rotated;
    
    new void OnEnable()
    {
        base.OnEnable();
        rotated = false;
        turnSpd = 0;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (!rotated)
        {
            trfm.Rotate(Vector3.forward * Random.Range(0, 360));
            rotated = true;
        }
        turnSpd += .003f;
        trfm.up += (Player.trfm.position - trfm.position) * turnSpd;
        trfm.position += trfm.up * spd;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 11)
        {
            Player.CollectShard(3);
            Destantiate();
        }
    }
}