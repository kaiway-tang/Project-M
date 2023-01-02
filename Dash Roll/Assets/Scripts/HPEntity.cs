using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEntity : MonoBehaviour
{
    [SerializeField] protected int HP, maxHP, deathTraumaAmount, deathSleepAmount, stunned, movementLocked;
    [SerializeField] protected Transform trfm;
    public EntityTypes entityID;

    [SerializeField] HPBar hpBar;

    public enum EntityTypes { Enemy, Player, None }
    public const int IGNORED = -1, ALIVE = 0, DEAD = 1;

    protected bool tookKnockback, tookDamage; protected Vector2 lastKnockback;

    public static ObjectPooler bloodFXPooler;

    protected void FixedUpdate()
    {
        if (stunned > 0) { stunned--; }
        if (movementLocked > 0) { movementLocked--; }
    }

    public int TakeDamage(int amount, EntityTypes ignoreEntity) //returns true if entity killed
    {
        if (ignoreEntity == entityID) { return IGNORED; }

        HP -= amount;

        if (HP <= 0)
        {
            End();
            return DEAD;
        }

        tookDamage = true;
        return ALIVE;
    }

    public int TakeDamage(int amount, EntityTypes ignoreEntity, Vector2 knockback) //returns true if entity killed
    {
        int result = TakeDamage(amount, ignoreEntity);

        bloodFXPooler.Instantiate(trfm.position, 0);

        if (result == ALIVE)
        {
            tookKnockback = true;
            lastKnockback = knockback;
            hpBar.SetPercentage((float)HP / maxHP);
        }
        return result;
    }

    public int TakeDamage(int amount, EntityTypes ignoreEntity, Vector3 source, int power) //returns true if entity killed
    {
        return TakeDamage(amount, ignoreEntity, (trfm.position - source).normalized * power);
    }

    public virtual void End()
    {
        CameraController.SetTrauma(deathTraumaAmount);
        CameraController.Sleep(deathSleepAmount);
        Destroy(transform.root.gameObject);
    }

    public void Heal(int amount, bool allowOverheal)
    {
        HP += amount;

        if(HP > maxHP)
        {
            if (!allowOverheal)
            {
                HP = maxHP;
            }
        }
    }

    public void Stun(int duration)
    {
        if (stunned < duration) { stunned = duration; }
    }

    public void LockMovement(int duration)
    {
        if (movementLocked < duration) { movementLocked = duration; }
    }
}
