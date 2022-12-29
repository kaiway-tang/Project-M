using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEntity : MonoBehaviour
{
    [SerializeField] protected int HP, maxHP, stunned, movementLocked;
    public EntityTypes entityID;

    public enum EntityTypes { Enemy, Player, None }

    protected void FixedUpdate()
    {
        if (stunned > 0) { stunned--; }
        if (movementLocked > 0) { movementLocked--; }
    }

    public void TakeDamage(int amount, EntityTypes ignoreEntity)
    {
        if (ignoreEntity == entityID) { return; }

        HP -= amount;

        if (HP <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
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
