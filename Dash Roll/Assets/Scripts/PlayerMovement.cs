using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MobileEntity
{
    [SerializeField] Vector2 velocity;
    [SerializeField] float acceleration, maxSpd, friction, airAcceleration, airMaxSpd, airFriction, jumpPower, wallKickVelocity, dashRollVelocity, dashRollFriction;
    [SerializeField] int jumps;
    [SerializeField] PlayerAnimator animator;

    [SerializeField] ParticleSystem dashPtclFX;
    [SerializeField] TrailRenderer[] dashTrailFX;
    [SerializeField] Collider2D hurtbox;
    [SerializeField] ObjectPooler dashShadowFXPooler;
    [SerializeField] Vector3 dashShadowFXOffset;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color dashBlack;

    int wallKickFXTimer, wallKickWindow, wallKickScreenShakeCooldown, hover;
    int attackCooldown, attackReset;
    int dashRollCooldown;
    bool refundJump;

    int dodgeFXActive;

    bool cameraTrackingIncreased;

    [SerializeField] DirectionalAttack attack1, attack2;

    Vector2 vect2; //passive vect2 to avoid declaring new Vector2 repeatedly
    Vector3 vect3; // ^^

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

    private new void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsTouchingGround())
        {
            if (jumps < 2) { jumps = 2; }
            if (cameraTrackingIncreased)
            {
                CameraController.self.trackingRate /= 2;
                cameraTrackingIncreased = false;
            }
        }

        MovementHandling();

        FrictionHandling();

        Clocks();
    }

    void Clocks()
    {
        if (wallKickFXTimer > 0) 
        {
            wallKickFXTimer--; 
            if (wallKickFXTimer == 4) { DisableDodgeFX(); }
            if (wallKickFXTimer == 0) { hurtbox.enabled = true; }
        }
        if (wallKickScreenShakeCooldown > 0) { wallKickScreenShakeCooldown--; }

        if (wallKickWindow > 0)
        {
            wallKickWindow--;
            if (wallKickWindow == 0) { refundJump = false; }
        }

        if (attackReset > 0)
        {
            attackReset--;
            if (attackReset == 17)
            {
                attack1.Activate(IsFacingRight());
            }
            if (attackReset == 9)
            {
                attack1.Deactivate();
            }
            if (attackReset == 0)
            {
                attackCooldown = 10;
            }
        }

        if (attackCooldown > 0)
        {
            attackCooldown--;
            if (attackCooldown == 21)
            {
                attack2.Activate(IsFacingRight());
            }
            if (attackCooldown == 12)
            {
                attack2.Deactivate();
            }
        }

        if (dashRollCooldown > 0)
        {
            dashRollCooldown--;

            if (dashRollCooldown > 34 && dashRollCooldown % 2 == 0)
            {
                //dashShadowFXPooler.Instantiate(trfm.position + dashShadowFXOffset, (50-dashRollCooldown)/2 * -45);
                //dashShadowFXPooler.Instantiate(trfm.position + dashShadowFXOffset);
            }

            if (dashRollCooldown == 34)
            {
                DisableDodgeFX();
            }
            if (dashRollCooldown == 30)
            {
                hurtbox.enabled = true;
            }
        }

        if (hover > 0)
        {
            hover--;
            SetYVelocity(0);
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
                        animator.RequestAnimatorState(PlayerAnimator.RUN);
                    }
                    else
                    {
                        animator.RequestAnimatorState(PlayerAnimator.IDLE);
                    }
                }
                else if (PlayerInput.LeftHeld())
                {
                    FaceLeft();
                    AddXVelocity(-acceleration, -maxSpd);
                    animator.RequestAnimatorState(PlayerAnimator.RUN);
                }
                else
                {
                    animator.RequestAnimatorState(PlayerAnimator.IDLE);
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
                    animator.RequestAnimatorState(PlayerAnimator.FALL);

                    if (rb.velocity.y < -30)
                    {
                        SetYVelocity(-30);
                        if (false && !cameraTrackingIncreased)
                        {
                            CameraController.self.trackingRate *= 2;
                            cameraTrackingIncreased = true;
                        }
                    }
                }
                else
                {
                    //left:
                    //front && left || back && right

                    //right:
                    //back && left || front && right

                    if (touchingTerrain[1])
                    {
                        animator.RequestAnimatorState(PlayerAnimator.CLING_FRONT);
                    }
                    else if (touchingTerrain[3])
                    {
                        animator.RequestAnimatorState(PlayerAnimator.CLING_BACK);
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
        if (dashRollCooldown > 39)
        {
            if (dashRollCooldown < 48)
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

    void AbilityHandling()
    {
        if (PlayerInput.AttackHeld())
        {
            if (attackCooldown < 1)
            {
                if (attackReset > 0)
                {
                    animator.QueAnimation(PlayerAnimator.ATTACK_2, 27);
                    LockMovement(27);
                    attackCooldown = 30;
                    attackReset = 0;

                    if (!IsTouchingGround()) { SetYVelocity(-30); }
                    AddForwardXVelocity(16, 16);
                }
                else
                {
                    animator.QueAnimation(PlayerAnimator.ATTACK_1, 15);
                    LockMovement(12);
                    attackCooldown = 15;
                    attackReset = 20;
                    AddForwardXVelocity(12, 12);

                    hover = 12;
                }
                //attackSlash.SetActive(true);
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

                animator.QueAnimation(PlayerAnimator.ROLL, 16);
                EnableDodgeFX();
                hurtbox.enabled = false;

                if (!IsTouchingGround()) { CameraController.SetTrauma(12); }

                rb.velocity = vect2 * dashRollVelocity;
                dashRollCooldown = 50;
            }
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

    void ApplyAirAnimation()
    {
        if (rb.velocity.y < 0)
        {
            animator.RequestAnimatorState(PlayerAnimator.FALL);
        }
        else
        {
            animator.RequestAnimatorState(PlayerAnimator.JUMP);
        }
    }

    void DoWallKick(float velocity)
    {
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
            animator.RequestAnimatorState(PlayerAnimator.JUMP);
        }
        else
        {
            animator.QueAnimation(PlayerAnimator.ROLL, 16);
        }
        if (rb.velocity.y < pJumpPower) 
        {
            SetYVelocity(pJumpPower);
        }
    }
}
