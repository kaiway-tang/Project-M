using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MobileEntity
{
    [SerializeField] float friction;
    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        ApplyXFriction(friction);
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
}
