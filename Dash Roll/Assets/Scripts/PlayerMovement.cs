using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MobileEntity
{
    [SerializeField] Vector2 velocity;
    [SerializeField] float acceleration, maxSpd, friction, airAcceleration, airMaxSpd, airFriction, jumpPower, wallKickVelocity, dashRollVelocity, dashRollFriction;
    [SerializeField] int jumps;
    [SerializeField] PlayerAnimator animator;

    [SerializeField] ParticleSystem dashPtclFX, dashRefreshFX;
    [SerializeField] TrailRenderer[] dashTrailFX;
    [SerializeField] Collider2D hurtbox;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color dashBlack;

    [SerializeField] GameObject kickFX;
    [SerializeField] ParticleSystem turnFX;
    [SerializeField] SimpleAnimation smashFX;

    int wallKickFXTimer, wallKickWindow, wallKickScreenShakeCooldown, attackKickTimer, dodgeFXActive;
    int attackCooldown, attackReset, attackPhase, attackHitboxTimer;
    int dashRollCooldown;
    bool refundJump;

    public int hover;

    bool cameraTrackingIncreased;

    [SerializeField] DirectionalAttack lightAttack, heavyAttack, kickAttack;
    [SerializeField] Transform cameraTargetPoint;
    int cameraTargetResetTimer;

    public new static Transform trfm;
    public static Transform headTrfm;
    void Awake()
    {
        trfm = transform;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity;

        JumpHandling();

        UpdateAbilityHandling();

        WallKickWindowHandling();
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsTouchingGround())
        {
            if (jumps < 2) { jumps = 2; }
        }

        FixedUpdateAbilityHandling();

        MovementHandling();

        FrictionHandling();

        Clocks();
    }

    void Clocks()
    {
        if (cameraTargetResetTimer > 0)
        {
            if (cameraTargetResetTimer == 1)
            {
                cameraTargetPoint.localPosition = new Vector2(2, 4);
            }
            cameraTargetResetTimer--;
        }

        if (wallKickFXTimer > 0) 
        {
            if (wallKickFXTimer == 5) { DisableDodgeFX(); }
            if (wallKickFXTimer == 1) { hurtbox.enabled = true; }
            wallKickFXTimer--; 
        }
        if (wallKickScreenShakeCooldown > 0) { wallKickScreenShakeCooldown--; }

        if (wallKickWindow > 0)
        {
            if (wallKickWindow == 1) { refundJump = false; }
            wallKickWindow--;
        }

        if (attackReset > 0)
        {
            if (attackReset == 1)
            {
                attackPhase = 0;
            }
            attackReset--;
        }

        if (attackHitboxTimer > 0)
        {
            if (attackHitboxTimer == 5)
            {
                if (attackPhase == 0) { heavyAttack.Activate(IsFacingRight()); }
                else { lightAttack.Activate(IsFacingRight()); }
            }
            if (attackHitboxTimer == 4)
            {
                lightAttack.Deactivate();
            }
            if (attackHitboxTimer == 3)
            {
                heavyAttack.Deactivate();
            }
            attackHitboxTimer--;
        }

        if (attackCooldown > 0)
        {
            attackCooldown--;
        }

        if (dashRollCooldown > 0)
        {
            if (dashRollCooldown == 1)
            {
                dashRefreshFX.Play();
            }

            dashRollCooldown--;

            if (dashRollCooldown == 59)
            {
                DisableDodgeFX();
            }
            if (dashRollCooldown == 55)
            {
                hurtbox.enabled = true;
            }
        }

        if (attackKickTimer > 0)
        {
            attackKickTimer--;
            if (attackKickTimer == 0)
            {
                kickAttack.Deactivate();
            }
        }

        if (hover > 0)
        {
            if (rb.velocity.y < 0) { SetYVelocity(0); }
            hover--;
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
                        if (IsFacingLeft())
                        {
                            FaceRight();
                            if (rb.velocity.x < -9)
                            {
                                turnFX.Play();
                            }
                        }
                        AddXVelocity(acceleration, maxSpd);
                        animator.RequestAnimatorState(animator.Run);
                    }
                    else
                    {
                        animator.RequestAnimatorState(animator.Idle);
                    }
                }
                else if (PlayerInput.LeftHeld())
                {
                    if (IsFacingRight())
                    {
                        FaceLeft();
                        if (rb.velocity.x > 9 )
                        {
                            turnFX.Play();
                        }
                    }
                    AddXVelocity(-acceleration, -maxSpd);
                    animator.RequestAnimatorState(animator.Run);
                }
                else
                {
                    animator.RequestAnimatorState(animator.Idle);
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

                if (rb.velocity.y < -15)
                {
                    animator.RequestAnimatorState(animator.Fall);

                    if (rb.velocity.y < -40)
                    {
                        cameraTargetResetTimer = 5;
                        vect2.x = 1; vect2.y = 44 + rb.velocity.y;
                        cameraTargetPoint.localPosition = vect2;

                        if (rb.velocity.y < -50)
                        {
                            SetYVelocity(-50);
                        }
                    }
                }
                else
                {
                    if (touchingTerrain[1])
                    {
                        animator.RequestAnimatorState(animator.ClingFront);
                    }
                    else if (touchingTerrain[3])
                    {
                        animator.RequestAnimatorState(animator.ClingBack);
                    }
                    else
                    {
                        ApplyAirAnimation();
                    }
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
        if (dashRollCooldown > 64)
        {
            if (dashRollCooldown < 73)
            {
                ApplyDirectionalFriction(dashRollFriction);
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

    void FixedUpdateAbilityHandling()
    {
        if (PlayerInput.AttackHeld() && dodgeFXActive < 1)
        {
            if (attackCooldown < 1)
            {
                if (attackPhase == 0) //light attack 1
                {
                    animator.QueAnimation(animator.LightAttack2, 12);
                    LockMovement(9);

                    attackCooldown = 12;
                    attackReset = 20;
                    attackHitboxTimer = 8;

                    AddForwardXVelocity(18, 18);
                    attackPhase = 1;
                }
                else if (attackPhase == 1) //light attack 2
                {
                    animator.QueAnimation(animator.LightAttack1, 15);
                    LockMovement(12);

                    attackCooldown = 15;
                    attackReset = 20;
                    attackHitboxTimer = 8;

                    AddForwardXVelocity(18, 18);
                    attackPhase = 2;
                }
                else if (attackPhase == 2) //heavy attack
                {
                    animator.QueAnimation(animator.HeavyAttack, 26);
                    LockMovement(27);

                    attackCooldown = 30;
                    attackReset = 0;
                    attackHitboxTimer = 11;

                    if (!IsTouchingGround()) { SetYVelocity(-40); }
                    AddForwardXVelocity(21, 21);
                    attackPhase = 0;
                }
            }
        }

        if (PlayerInput.DashRollHeld())
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

                animator.QueAnimation(animator.Roll, 16);
                EnableDodgeFX();
                hurtbox.enabled = false;

                if (!IsTouchingGround()) { CameraController.SetTrauma(9); }

                rb.velocity = vect2 * dashRollVelocity;
                dashRollCooldown = 75;
            }
        }
    }

    void UpdateAbilityHandling()
    {
        if (dodgeFXActive > 0 && PlayerInput.AttackPressed())
        {
            animator.QueAnimation(animator.Kick, 8);
            Instantiate(kickFX, trfm.position, trfm.rotation);
            kickAttack.Activate(IsFacingRight());
            attackKickTimer = 8;
        }
    }

    void EnableDodgeFX()
    {
        if (dodgeFXActive < 1)
        {
            dashPtclFX.Play();
            dashTrailFX[0].emitting = true;
            dashTrailFX[1].emitting = true;
            spriteRenderer.color = dashBlack;
        }
        dodgeFXActive++;
    }

    void DisableDodgeFX()
    {
        dodgeFXActive--;
        if (dodgeFXActive < 1)
        {
            dashPtclFX.Stop();
            dashTrailFX[0].emitting = false;
            dashTrailFX[1].emitting = false;
            spriteRenderer.color = Color.white;
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
                DoJump(jumpPower * .9f);

                CameraController.SetTrauma(6);
            }
            else if (jumps == 1)
            {
                jumps--;
                refundJump = true;
                DoJump(jumpPower * .8f);

                CameraController.SetTrauma(4);
            }

            wallKickWindow = 5;
        }
    }

    void ApplyAirAnimation()
    {
        if (rb.velocity.y < 0)
        {
            animator.RequestAnimatorState(animator.Fall);
        }
        else
        {
            animator.RequestAnimatorState(animator.Jump);
        }
    }

    void DoWallKick(float velocity)
    {
        animator.QueAnimation(animator.Roll, 16);

        DoJump(jumpPower);
        SetXVelocity(velocity);
        wallKickWindow = 0;

        if (wallKickScreenShakeCooldown < 1) { CameraController.SetTrauma(12); }
        else { CameraController.SetTrauma(8); }
        wallKickScreenShakeCooldown = 30;

        if (wallKickFXTimer < 5) { EnableDodgeFX(); }
        hurtbox.enabled = false;
        wallKickFXTimer = 20;
    }

    bool CanWallKick()
    {
        return touchingTerrain[4] || (!IsTouchingGround() && touchingTerrain[3]);
    }

    void DoJump(float pJumpPower)
    {
        if (IsTouchingGround())
        {
            animator.RequestAnimatorState(animator.Jump);
        }
        else
        {
            animator.QueAnimation(animator.Roll, 16);
        }
        if (rb.velocity.y < pJumpPower) 
        {
            SetYVelocity(pJumpPower);
        }
    }
}
