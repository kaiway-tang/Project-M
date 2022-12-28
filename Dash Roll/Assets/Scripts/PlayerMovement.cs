using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MobileEntity
{
    [SerializeField] Vector2 velocity;
    [SerializeField] float acceleration, maxSpd, friction, airAcceleration, airMaxSpd, airFriction, jumpPower, wallKickVelocity, dashRollVelocity, dashRollFriction;
    [SerializeField] int jumps;
    [SerializeField] Animator animator;
    [SerializeField] int animatorState;
    const int IDLE = 0, RUN = 1, JUMP = 2, FALL = 3;

    int wallKickCooldown, wallKickWindow, wallKickScreenShakeCooldown;
    int attackCooldown, dashRollCooldown;
    bool refundJump;

    [SerializeField] GameObject attackSlash;

    Vector2 vect2;
    Vector3 vect3;

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
        if (wallKickScreenShakeCooldown > 0) { wallKickScreenShakeCooldown--; }

        if (wallKickWindow > 0) 
        {
            wallKickWindow--; 
            if (wallKickWindow == 0) { refundJump = false; }
        }

        if (attackCooldown > 0)
        {
            if (attackCooldown == 20)
            {
                attackSlash.SetActive(false);
            }
            attackCooldown--;
        }

        if (dashRollCooldown > 0)
        {
            dashRollCooldown--;
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
                        FaceRight();
                        AddXVelocity(acceleration, maxSpd);
                        SetAnimatorState(RUN);
                    }
                    else
                    {
                        SetAnimatorState(IDLE);
                    }
                }
                else if (PlayerInput.LeftHeld())
                {
                    FaceLeft();
                    AddXVelocity(-acceleration, -maxSpd);
                    SetAnimatorState(RUN);
                }
                else
                {
                    SetAnimatorState(IDLE);
                }
            }
            else
            {
                if (PlayerInput.RightHeld())
                {
                    if (!PlayerInput.LeftHeld())
                    {
                        FaceRight();
                        AddXVelocity(airAcceleration, airMaxSpd);
                    }
                }
                else if (PlayerInput.LeftHeld())
                {
                    FaceLeft();
                    AddXVelocity(-airAcceleration, -airMaxSpd);
                }

                if (rb.velocity.y < 0)
                {
                    SetAnimatorState(FALL);
                }
            }
        }
    }

    void WallKickWindowHandling()
    {
        if (wallKickWindow > 0)
        {
            if (CanWallKick())
            {
                if (PlayerInput.RightHeld())
                {
                    DoWallKick(wallKickVelocity);
                    if (refundJump) { jumps++; }
                }
                if (PlayerInput.LeftHeld())
                {
                    DoWallKick(-wallKickVelocity);
                    if (refundJump) { jumps++; }
                }
            }
        }
    }

    void FrictionHandling()
    {
        if (dashRollCooldown > 39)
        {
            if (dashRollCooldown < 48)
            {
                ApplyDirectionalFriction(dashRollFriction);
            }

            if (dashRollCooldown == 40)
            {
                //EnableGravity();
            }
        }
        else
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
    }

    void AbilityHandling()
    {
        if (PlayerInput.AttackPressed())
        {
            if (attackCooldown < 1)
            {
                attackSlash.SetActive(true);
                attackCooldown = 25;
            }
        }

        if (PlayerInput.DashRollPressed())
        {
            if (dashRollCooldown < 1)
            {
                vect2 = PlayerInput.GetVectorInput();

                if (vect2.y == 0)
                {
                    vect2.y = .15f;
                    if (vect2.x == 0)
                    {
                        if (IsFacingLeft()) { vect2.x = -1; }
                        else { vect2.x = 1; }
                    }
                }

                //DisableGravity();
                if (!IsTouchingGround()) { CameraController.SetTrauma(12); }

                rb.velocity = vect2 * dashRollVelocity;
                dashRollCooldown = 50;
            }
        }
    }

    void JumpHandling()
    {
        if (PlayerInput.JumpPressed())
        {
            if (CanWallKick())
            {
                if (PlayerInput.RightHeld()) 
                {
                    DoWallKick(wallKickVelocity); return; 
                }
                if (PlayerInput.LeftHeld()) 
                {
                    DoWallKick(-wallKickVelocity); return; 
                }
            }

            if (IsTouchingGround())
            {
                DoJump(jumpPower);
            }
            else if (jumps > 1)
            {

                jumps--;
                refundJump = true;
                DoJump(jumpPower * .8f);

                CameraController.SetTrauma(8);
            }
            else if (jumps == 1)
            {
                jumps--;
                refundJump = true;
                DoJump(jumpPower * .6f);

                CameraController.SetTrauma(6);
            }

            wallKickWindow = 5;
        }
    }

    void DoWallKick(float velocity)
    {
        if (wallKickCooldown < 1)
        {
            DoJump(jumpPower);
            SetXVelocity(velocity);
            wallKickWindow = 0;

            if (wallKickScreenShakeCooldown < 1) { CameraController.SetTrauma(12); }
            else { CameraController.SetTrauma(8); }
            wallKickScreenShakeCooldown = 30;

            //jumps = 1;
            //wallKickCooldown = 10;
        }
    }

    bool CanWallKick()
    {
        return touchingTerrain[4] || (!IsTouchingGround() && touchingTerrain[3]);
    }

    void DoJump(float pJumpPower)
    {
        SetAnimatorState(JUMP);
        if (rb.velocity.y < pJumpPower) 
        {
            SetYVelocity(pJumpPower);
        }
    }
    void SetAnimatorState(int state)
    {
        if (animatorState != state)
        {
            animator.SetInteger("State", state);
            animatorState = state;
        }
    }
}
