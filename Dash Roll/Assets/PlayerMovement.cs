using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MobileEntity
{
    [SerializeField] Vector2 velocity;

    [SerializeField] float acceleration, maxSpd, airAcceleration, airMaxSpd, jumpPower, friction;

    int jumps;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity;

        if (PlayerInput.UpPressed())
        {
            if (isOnGround)
            {
                DoJump();
            }
            else if (jumps > 0)
            {
                jumps--;
                DoJump();
            }
        }
    }

    private void FixedUpdate()
    {
        if (jumps < 1 && isOnGround) { jumps = 1; }

        AbilityHandling();

        MovementHandling();

        ApplyXFriction(friction);
    }

    void MovementHandling()
    {
        if (movementLocked < 1)
        {
            if (PlayerInput.RightHeld())
            {
                if (!PlayerInput.LeftHeld())
                {
                    facing = FACING_RIGHT;
                    AddXVelocity(acceleration, maxSpd);
                }
            }
            else if (PlayerInput.LeftHeld())
            {
                facing = FACING_LEFT;
                AddXVelocity(-acceleration, -maxSpd);
            }
        }
    }
    void AbilityHandling()
    {
        if (PlayerInput.DashRollPressed())
        {

        }
    }

    void DoJump()
    {
        if (rb.velocity.y < 0) { SetYVelocity(0); }
        AddYVelocity(jumpPower, jumpPower * 3);
    }
}
