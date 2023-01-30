using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEntity : MonoBehaviour
{
    [SerializeField] protected int HP, maxHP, deathTraumaAmount, deathSleepAmount, stunned, movementLocked, poisoned, invulnerable;
    public Transform trfm;
    public EntityTypes entityID;

    [SerializeField] HPBar hpBar;

    public enum EntityTypes { Enemy, Player, Neutral }
    public enum DamageSource { Default, LightAttack1, LightAttack2, HeavyAttack, UpAttack, DownAttack, KickAttack }
    DamageSource lastDamageSource;
    public const int IGNORED = -1, ALIVE = 0, DEAD = 1;

    protected bool tookKnockback, tookDamage;
    protected Vector2 lastKnockback;
    protected int lastDamage, ignoreLastDamageSource;

    public static ObjectPooler bloodFXPooler;
    [SerializeField] GameObject damagedFXObj;
    [SerializeField] ObjectPooler damagedFXPooler;
    public bool isStructure, grantsMana = true;
    [SerializeField] bool standardDestroyOnDeath; //structures vulnerable to kicks
    bool usingInstantiateDamagedFX, usingHPBar;

    protected void Start()
    {
        if (damagedFXObj)
        {
            usingInstantiateDamagedFX = true;
        }
        else if (!damagedFXPooler)
        {
            damagedFXPooler = bloodFXPooler;
        }

        usingHPBar = hpBar;
    }

    protected void FixedUpdate()
    {
        if (tookDamage) { tookDamage = false; }

        if (stunned > 0) { stunned--; }
        if (movementLocked > 0) { movementLocked--; }
        if (poisoned > 0)
        {
            if (poisoned % 50 == 1)
            {
                if (entityID == EntityTypes.Player && HP < 6)
                {
                    TakeDamage(HP - 1, EntityTypes.Neutral);
                }
                else
                {
                    TakeDamage(5, EntityTypes.Neutral);
                }
            }
            poisoned--;
        }
        if (invulnerable > 0) { invulnerable--; }
        if (ignoreLastDamageSource > 0)
        {
            if (ignoreLastDamageSource == 1) { lastDamageSource = DamageSource.Default; }
            ignoreLastDamageSource--;
        }
    }

    public int GetHP() { return HP; }

    public int TakeDamage(int amount, EntityTypes entitySource, DamageSource damageSource = DamageSource.Default) //returns true if entity killed
    {
        if (entitySource == entityID || invulnerable > 0) { return IGNORED; }
        if (damageSource != DamageSource.Default)
        {
            ignoreLastDamageSource = 15;
            if (lastDamageSource == damageSource)
            {
                return IGNORED;
            }
            else
            {
                lastDamageSource = damageSource;
            }
        }

        if (isStructure && damageSource == DamageSource.KickAttack) { HP -= amount * 5; }
        else { HP -= amount; }

        if (usingInstantiateDamagedFX)
        {
            Instantiate(damagedFXObj, trfm.position, Quaternion.identity);
        }
        else
        {
            damagedFXPooler.Instantiate(trfm.position, 0);
        }

        tookDamage = true;
        lastDamage = amount;

        if (HP <= 0)
        {
            if (standardDestroyOnDeath) { Destroy(trfm.gameObject); }
            return DEAD;
        }
        else
        {
            if (entityID != EntityTypes.Player && usingHPBar) { hpBar.SetPercentage((float)HP / maxHP); }
            return ALIVE;
        }
    }

    public int TakeDamage(int amount, EntityTypes entitySource, Vector2 knockback, DamageSource damageSource = DamageSource.Default) //returns true if entity killed
    {
        int result = TakeDamage(amount, entitySource, damageSource);

        if (result == ALIVE)
        {
            tookKnockback = true;
            lastKnockback = knockback;
        }
        return result;
    }

    public int TakeDamage(int amount, EntityTypes entitySource, Vector3 source, int power) //returns true if entity killed
    {
        return TakeDamage(amount, entitySource, (trfm.position - source).normalized * power);
    }

    public void End()
    {
        CameraController.SetTrauma(deathTraumaAmount);
        CameraController.Sleep(deathSleepAmount);
        Destroy(trfm.gameObject);
    }

    public void Heal(int amount, bool allowOverheal = false)
    {
        HP += amount;

        if (HP > maxHP)
        {
            if (!allowOverheal)
            {
                HP = maxHP;
            }
        }

        if (usingHPBar) { hpBar.SetPercentage((float)HP / maxHP); }
    }

    public void Stun(int duration)
    {
        if (stunned < duration) { stunned = duration; }
        if (movementLocked < stunned) { movementLocked = stunned; }
    }

    public void LockMovement(int duration)
    {
        if (movementLocked < duration) { movementLocked = duration; }
    }
    public int Poison(int seconds, EntityTypes ignoreEntity, bool stacking = false)
    {
        if (ignoreEntity == entityID) { return IGNORED; }

        if (stacking)
        {
            poisoned += 50 * seconds;
        }
        else
        {
            if (poisoned < 50 * seconds)
            {
                poisoned += 50 * seconds;
                if (poisoned > 50 * seconds)
                {
                    poisoned = 50 * seconds;
                }
            }
        }
        return ALIVE;
    }
}
