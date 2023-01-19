using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MobileEntity
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
    [SerializeField] ParticleSystem turnFX, frontTurnFX, runFX;
    [SerializeField] SimpleAnimation smashFX;
    public static ObjectPooler jumpRingFXPooler;

    [SerializeField] GameObject shootingSword;

    int wallKickFXTimer, wallKickWindow, wallKickScreenShakeCooldown, attackKickTimer, dodgeFXActive, clingAnimationDelay;
    int attackCooldown, attackReset, attackPhase, attackHitboxTimer;
    int dashRollCooldown, kickAttackCooldown;
    int manaTimer, castWindup, castCooldown;
    bool refundJump, highFall, castQued, runFXActive;

    public int hover, deathAnimationTimer;

    [SerializeField] DirectionalAttack lightAttack1, lightAttack2, heavyAttack, kickAttack;
    [SerializeField] Transform cameraTargetPoint;
    int cameraTargetResetTimer;

    public new static Transform trfm;
    public static Transform headTrfm;
    public static Player playerScript;

    static bool firstStartComplete, returnToObeliskOnStart;
    public static int m_HP, mana, soulShards, maxShards, currentShardPercent;

    public static Vector3 lastSafePosition;

    [SerializeField] PlayerResourceBar playerHPBar, manaBar;
    void Awake()
    {
        trfm = base.trfm;
        playerScript = GetComponent<Player>();

        if (returnToObeliskOnStart)
        {
            trfm.position = Obelisk.lastObeliskPosition;
            returnToObeliskOnStart = false;
        }
        else
        {
            trfm.position = GameManager.spawnPosition;
        }
    }
    new void Start()
    {
        if (!firstStartComplete)
        {
            mana = 100;
            m_HP = HP;
            maxShards = 3;

            firstStartComplete = true;
        }
        else
        {
            HP = m_HP;
        }

        lastSafePosition = trfm.position;

        base.Start();
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

        if (tookDamage)
        {
            if ((int)(lastDamage * 1.2f) < 10)
            {
                CameraController.AddTrauma(10);
                HUDManager.SetVignetteOpacity(.3f);
            }
            else 
            {
                invulnerable = 2;
                CameraController.SetTrauma((int)(lastDamage * 1));
                HUDManager.SetVignetteOpacity(lastDamage * .03f);
            }

            playerHPBar.SetPercentage((float)HP/maxHP);

            if (HP <= 0) 
            {
                playerHPBar.SetPercentage(0);
                HP = 0;

                poisoned = 0;
                animator.QueAnimation(animator.Death, 300);
                DisableGravity();
                DisableHurtbox();
                HUDManager.SetBlackCoverOpacity(1);
                deathAnimationTimer = 350;
                CameraController.AddTrauma(15);
                spriteRenderer.sortingLayerID = 94093041;
                spriteRenderer.sortingOrder = 999;
                Stun(325);
            }

            tookDamage = false;
        }

        base.FixedUpdate();

        if (IsTouchingGround())
        {
            if (jumps < 2) { jumps = 2; }
        }

        FixedUpdateAbilityHandling();

        MovementHandling();

        FrictionHandling();

        HandlePositionPredicting();

        HandleDeathAnimation();

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
            if (wallKickFXTimer == 1) { EnableHurtbox(); }
            wallKickFXTimer--; 
        }
        if (wallKickScreenShakeCooldown > 0) { wallKickScreenShakeCooldown--; }

        if (wallKickWindow > 0)
        {
            if (wallKickWindow == 1 && refundJump)
            {
                jumpRingFXPooler.Instantiate(trfm.position - Vector3.up * .2f, 0);
                refundJump = false; 
            }
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
            if (attackHitboxTimer == 6)
            {
                if (attackPhase == 0) { heavyAttack.Activate(IsFacingRight()); }
                else if (attackPhase == 1) { lightAttack1.Activate(IsFacingRight()); }
                else if (attackPhase == 2) { lightAttack2.Activate(IsFacingRight()); }
            }
            if (attackHitboxTimer == 1)
            {
                heavyAttack.Deactivate();
                lightAttack1.Deactivate();
                lightAttack2.Deactivate();
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
                EnableHurtbox();
            }
        }

        if (kickAttackCooldown > 0) { kickAttackCooldown--; }

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
        
        if (manaTimer > 0) { manaTimer--; }
        else if (mana > 0)
        {
            AddMana(-1);
            manaTimer = 25;
        }

        if (clingAnimationDelay > 0)
        {
            clingAnimationDelay -= 9;

            if (clingAnimationDelay < 0)
            {
                clingAnimationDelay = 0;
            }
            else if (clingAnimationDelay > 8)
            {
                if (touchingTerrain[1])
                {
                    animator.RequestAnimatorState(animator.ClingFront);
                }
                else if (touchingTerrain[3])
                {
                    animator.RequestAnimatorState(animator.ClingBack);
                }
            }
        }
    }

    void HandleDeathAnimation()
    {
        if (deathAnimationTimer > 0)
        {
            if (deathAnimationTimer == 347) { HUDManager.self.blackCoverSpriteRenderer.color = Color.black; }
            if (deathAnimationTimer > 150 && deathAnimationTimer < 251)
            {
                spriteRenderer.color -= new Color(0,0,0,.01f);
            }
            if (deathAnimationTimer == 100)
            {
                EnableGravity();
                EnableHurtbox();
                AddMana(999);
                Heal(999);
                returnToObeliskOnStart = true;
                GameManager.LoadScene(Obelisk.lastObeliskScene);
            }
            deathAnimationTimer--;
        }
    }

    void MovementHandling()
    {
        if (IsTouchingGround())
        {
            if (highFall)
            {
                turnFX.Play();
                frontTurnFX.Play();
                highFall = false;
            }

            if (movementLocked < 1)
            {
                if (PlayerInput.RightHeld())
                {
                    if (!PlayerInput.LeftHeld())
                    {
                        if (IsFacingLeft())
                        {
                            FaceRight();
                            if (rb.velocity.x < -6)
                            {
                                turnFX.Play();
                            }
                        }
                        AddXVelocity(acceleration, maxSpd);
                        animator.RequestAnimatorState(animator.Run);
                        EnableRunFX();
                    }
                    else
                    {
                        animator.RequestAnimatorState(animator.Idle);
                        DisableRunFX();
                    }
                }
                else if (PlayerInput.LeftHeld())
                {
                    if (IsFacingRight())
                    {
                        FaceLeft();
                        if (rb.velocity.x > 6)
                        {
                            turnFX.Play();
                        }
                    }
                    AddXVelocity(-acceleration, -maxSpd);
                    animator.RequestAnimatorState(animator.Run);
                    EnableRunFX();
                }
                else
                {
                    animator.RequestAnimatorState(animator.Idle);
                    DisableRunFX();
                }
            }
            else
            {
                animator.RequestAnimatorState(animator.Idle);
                DisableRunFX();
            }
        }
        else
        {
            DisableRunFX();

            if (movementLocked < 1)
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
                        highFall = true;
                        SetYVelocity(-50);
                    }
                    else
                    {
                        highFall = false;
                    }
                }
            }
            else
            {
                if (touchingTerrain[1] || touchingTerrain[3])
                {
                    clingAnimationDelay += 10;
                }
                else
                {
                    ApplyAirAnimation();
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
                    attackHitboxTimer = 9;

                    AddForwardXVelocity(18, 18);
                    attackPhase = 1;
                }
                else if (attackPhase == 1) //light attack 2
                {
                    animator.QueAnimation(animator.LightAttack1, 15);
                    LockMovement(12);

                    attackCooldown = 15;
                    attackReset = 20;
                    attackHitboxTimer = 9;

                    AddForwardXVelocity(18, 18);
                    attackPhase = 2;
                }
                else if (attackPhase == 2) //heavy attack
                {
                    animator.QueAnimation(animator.HeavyAttack, 26);
                    LockMovement(27);

                    attackCooldown = 30;
                    attackReset = 0;
                    attackHitboxTimer = 12;

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
                DisableHurtbox();

                if (!IsTouchingGround()) { CameraController.SetTrauma(9); }

                rb.velocity = vect2 * dashRollVelocity;
                dashRollCooldown = 75;
            }
        }

        if (PlayerInput.CastHeld() && mana >= 20)
        {
            if (castWindup < 1)
            {
                animator.QueAnimation(animator.CastHorizontal, 999);
                castWindup = 1;
            }
            if (!castQued)
            {
                movementLocked = 5;
                SetYVelocity(0);
                hover = 5;
                castQued = true;
                AddMana(-20);

                vect3.y = .7f; vect3.z = 0;
                if (IsFacingRight())
                {
                    vect3.x = 2.2f;
                    Instantiate(shootingSword, trfm.position + vect3, Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    vect3.x = -2.2f;
                    Instantiate(shootingSword, trfm.position + vect3, Quaternion.Euler(0, 0, 180));
                }
            }
        }

        if (castCooldown > 0) 
        {
            castCooldown--; 
        }
        else if (castWindup > 9)
        {
            if (castQued)
            {
                castCooldown = 5;
                AddForwardXVelocity(-11,11);
                castQued = false;
            }
            else
            {
                animator.DeQueAnimation(animator.CastHorizontal);
                castWindup = -10;
            }
        }

        if (castWindup < 10 && castWindup > 0) { castWindup++; }
    }

    void UpdateAbilityHandling()
    {
        if (dodgeFXActive > 0 && PlayerInput.AttackPressed() && kickAttackCooldown < 1)
        {
            animator.QueAnimation(animator.Kick, 8);
            if (IsFacingRight()) { Instantiate(kickFX, trfm.position + Vector3.right * -2, Quaternion.identity).transform.parent = trfm; }
            else { Instantiate(kickFX, trfm.position + Vector3.right * 2, Quaternion.Euler(0,0,180)).transform.parent = trfm; }
            kickAttack.Activate(IsFacingRight());
            attackKickTimer = 8;
            kickAttackCooldown = 10;
        }

        if (mana < 20 && PlayerInput.CastPressed())
        {
            HUDManager.FlashManaBar();
        }

        if (soulShards > 0 && PlayerInput.SoulPressed())
        {
            soulShards--;
            PlayerHeal((int)((maxHP-HP) * .5f));
            PlayerHeal((int)(maxHP * .1f));
            AddMana(999);
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
        if (movementLocked > 0) { return; }

        if (PlayerInput.JumpPressed())
        {
            if (CanWallKick())
            {
                if (PlayerInput.RightHeld()) 
                {
                    DoWallKick(wallKickVelocity); return; 
                } else if (PlayerInput.LeftHeld()) 
                {
                    DoWallKick(-wallKickVelocity); return;
                }
                else
                {
                    refundJump = !IsTouchingGround() && jumps > 0;
                    wallKickWindow = 5;
                }
            } else if (touchingTerrain[1])
            {
                refundJump = !IsTouchingGround() && jumps > 0;
                wallKickWindow = 5;
            }

            if (IsTouchingGround())
            {
                DoJump(jumpPower);
            }
            else if (jumps > 1)
            {

                jumps--;
                DoJump(jumpPower * .9f);

                CameraController.SetTrauma(6);

                if (wallKickWindow < 1) { jumpRingFXPooler.Instantiate(trfm.position - Vector3.up * .2f, 0); }
            }
            else if (jumps == 1)
            {
                jumps--;
                DoJump(jumpPower * .8f);

                CameraController.SetTrauma(4);

                if (wallKickWindow < 1) { jumpRingFXPooler.Instantiate(trfm.position - Vector3.up * .2f, 0); }
            }
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

        if (wallKickFXTimer < 5) 
        {
            EnableDodgeFX(); 
            if (wallKickFXTimer < 1)
            {
                DisableHurtbox();
            }
        }
        wallKickFXTimer = 20;
    }

    bool CanWallKick()
    {
        return (touchingTerrain[4] && !touchingTerrain[2]) || (!IsTouchingGround() && touchingTerrain[3]);
    }

    [SerializeField] int hurtboxDisable;
    void DisableHurtbox()
    {
        if (hurtboxDisable < 1) { hurtbox.enabled = false; }
        hurtboxDisable++;
    }

    void EnableHurtbox()
    {
        hurtboxDisable--;
        if (hurtboxDisable < 1) { hurtbox.enabled = true; }
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

    int velocityLogTimer;
    Vector2[] loggedVelocities = new Vector2[4];
    Vector2 averageVelocity;
    int nextIndex;
    void HandlePositionPredicting()
    {
        if (velocityLogTimer > 0) { velocityLogTimer--; }
        else {
            loggedVelocities[nextIndex] = rb.velocity;
            averageVelocity = (loggedVelocities[0] + loggedVelocities[1] + loggedVelocities[2] + loggedVelocities[3] + loggedVelocities[nextIndex]) * .2f;

            nextIndex++;
            if (nextIndex > 3) { nextIndex = 0; }

            velocityLogTimer = 5;
        }
    }

    public static Vector2 PredictedPosition(int ticks)
    {
        vect2 = trfm.position;
        return playerScript.averageVelocity * ticks * .02f + vect2;
    }

    public static void AddMana(int amount)
    {
        mana += amount;
        if (mana > 100) { mana = 100; }
        playerScript.manaBar.SetPercentage((float)mana * .01f);
    }
    public static void AddMana(int damage, int remainingHP)
    {
        if (remainingHP > damage) { AddMana(Mathf.RoundToInt(damage)); }
        else { AddMana(Mathf.RoundToInt(remainingHP)); }
    }

    public static void CollectShard(int amount)
    {
        currentShardPercent += amount;
        if (currentShardPercent > 99)
        {
            currentShardPercent -= 100;
            soulShards++;

            if (soulShards > maxShards) { soulShards = maxShards; }
        }
    }

    public static void PlayerHeal(int amount)
    {
        playerScript.Heal(amount);
        playerScript.playerHPBar.SetPercentage((float)playerScript.HP / playerScript.maxHP);
    }

    public bool isInVoid, reviving;

    private void OnBecameInvisible()
    {
        if (GameManager.nextScene != -1)
        {
            Invoke("LoadNextScene", 2);
            Stun(999);
            HUDManager.FadeBlackCoverOpacity(1);
        }
        if (isInVoid)
        {
            TakeDamage(20, HPEntity.EntityTypes.Neutral);

            if (HP > 0)
            {
                HUDManager.FadeBlackCoverOpacity(1);
                Stun(100);
                reviving = true;
                Invoke("Revive", 2);
            }
        }
    }
    void LoadNextScene()
    {
        GameManager.LoadNextScene();
    }

    public static void ReturnToLastSafePosition()
    {
        playerScript.rb.velocity = Vector2.zero;
        trfm.position = lastSafePosition;
    }

    void Revive()
    {
        reviving = false;
        isInVoid = false;
        ReturnToLastSafePosition();
        HUDManager.FadeBlackCoverOpacity(0);
        CameraController.self.mode = CameraController.TRACK_PLAYER;
    }

    void EnableRunFX()
    {
        if (!runFXActive)
        {
            runFX.Play();
            runFXActive = true;
        }
    }

    void DisableRunFX()
    {
        if (runFXActive)
        {
            runFX.Stop();
            runFXActive = false;
        }
    }
}
