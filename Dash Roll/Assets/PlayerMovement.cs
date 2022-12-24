using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MobileEntity
{
    [SerializeField] Vector2 velocity;

    [SerializeField] float acceleration, maxSpd, friction, airAcceleration, airMaxSpd, airFriction, jumpPower, wallKickVelocity, dashRollVelocity;

    [SerializeField] int jumps;

    int wallKickCooldown, wallKickWindow, dashRollCooldown;
    bool refundJump;

    Vector2 vect2;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity;

        JumpHandling();

        AbilityHandling();

        WallKickWindowHandling();
    }

    private void FixedUpdate()
    {

        if (jumps < 2 && IsTouchingGround()) { jumps = 2; }

        MovementHandling();

        FrictionHandling();

        Clocks();
    }

    void Clocks()
    {
        if (wallKickCooldown > 0) { wallKickCooldown--; }
        if (wallKickWindow > 0) 
        {
            wallKickWindow--; 
            if (wallKickWindow == 0) { refundJump = false; }
        }
        if (dashRollCooldown > 0)
        {
            dashRollCooldown--;
            if (dashRollCooldown > 24)
            {
                ApplyDirectionalFriction(friction);

                if (dashRollCooldown == 25)
                {
                    EnableGravity();
                }
            }
        }
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

            
        }
    }

    void WallKickWindowHandling()
    {
        if (wallKickWindow > 0)
        {
            if (IsTouchingLeftWall() && PlayerInput.RightHeld())
            {
                DoWallKick(wallKickVelocity);
                if (refundJump) { jumps++; }
            }
            else if (IsTouchingRightWall() && PlayerInput.LeftHeld())
            {
                DoWallKick(-wallKickVelocity);
                if (refundJump) { jumps++; }
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
            if (dashRollCooldown < 1)
            {
                vect2 = PlayerInput.GetVectorInput();

                if (vect2.y == 0)
                {
                    vect2.y = .1f;
                    if (vect2.x == 0)
                    {
                        if (IsFacingLeft()) { vect2.x = -1; }
                        else { vect2.x = 1; }
                    }
                }

                DisableGravity();

                rb.velocity = vect2 * dashRollVelocity;
                dashRollCooldown = 50;
            }
        }
    }

    void JumpHandling()
    {
        if (PlayerInput.JumpPressed())
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
                if (IsTouchingGround())
                {
                    DoJump(jumpPower);
                }
                else
                {
                    wallKickWindow = 5;

                    if (jumps == 2)
                    {

                        jumps--;
                        refundJump = true;
                        DoJump(jumpPower * .8f);
                    }
                    else if (jumps == 1)
                    {
                        jumps--;
                        refundJump = true;
                        DoJump(jumpPower * .6f);
                    }
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
            wallKickWindow = 0;

            //jumps = 1;
            //wallKickCooldown = 10;
        }
    }

    void DoJump(float pJumpPower)
    {
        if (rb.velocity.y < pJumpPower) { SetYVelocity(pJumpPower); }
    }
}
