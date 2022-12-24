using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MobileEntity
{
    [SerializeField] Vector2 velocity;

    [SerializeField] float acceleration, maxSpd, friction, airAcceleration, airMaxSpd, airFriction, jumpPower, wallKickVelocity;

    int jumps, wallKickCooldown, wallKickWindow;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity;

        JumpHandling();
    }

    private void FixedUpdate()
    {
        if (jumps < 1 && IsTouchingGround()) { jumps = 2; }

        AbilityHandling();

        MovementHandling();

        FrictionHandling();

        Clocks();
    }

    void Clocks()
    {
        if (wallKickCooldown > 0) { wallKickCooldown--; }
        if (wallKickWindow > 0) { wallKickWindow--; }
    }

    void MovementHandling()
    {
        if (movementLocked < 1)
        {
            if (IsTouchingGround())
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
            else
            {
                if (PlayerInput.RightHeld())
                {
                    if (!PlayerInput.LeftHeld())
                    {
                        facing = FACING_RIGHT;
                        AddXVelocity(airAcceleration, airMaxSpd);
                    }
                }
                else if (PlayerInput.LeftHeld())
                {
                    facing = FACING_LEFT;
                    AddXVelocity(-airAcceleration, -airMaxSpd);
                }
            }

            if (wallKickWindow > 0)
            {
                if (IsTouchingLeftWall() && PlayerInput.RightHeld())
                {
                    DoWallKick(wallKickVelocity);
                    jumps++;
                }
                else if (IsTouchingRightWall() && PlayerInput.LeftHeld())
                {
                    DoWallKick(-wallKickVelocity);
                    jumps++;
                }
            }
        }
    }

    void FrictionHandling()
    {
        if (IsTouchingGround())
        {
            ApplyXFriction(friction);
        }
        else
        {
            ApplyXFriction(airFriction);
        }
    }

    void AbilityHandling()
    {
        if (PlayerInput.DashRollPressed())
        {

        }
    }

    void JumpHandling()
    {
        if (PlayerInput.UpPressed())
        {
            if (IsTouchingLeftWall() && PlayerInput.RightHeld())
            {
                DoWallKick(wallKickVelocity);
            }
            else if (IsTouchingRightWall() && PlayerInput.LeftHeld())
            {
                DoWallKick(-wallKickVelocity);
            } else
            {
                wallKickWindow = 5;
                if (IsTouchingGround())
                {
                    DoJump(jumpPower);
                }
                else if (jumps == 2)
                {
                    jumps--;
                    DoJump(jumpPower * .8f);
                }
                else if (jumps == 1)
                {
                    jumps--;
                    DoJump(jumpPower * .6f);
                }
            }
        }
    }

    void DoWallKick(float velocity)
    {
        if (wallKickCooldown < 1)
        {
            DoJump(jumpPower);
            SetXVelocity(velocity);
            //jumps = 1;
            wallKickCooldown = 10;
        }
    }

    void DoJump(float pJumpPower)
    {
        if (rb.velocity.y < pJumpPower) { SetYVelocity(pJumpPower); }
    }
}
