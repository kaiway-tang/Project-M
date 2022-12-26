using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEntity : MonoBehaviour
{
    [SerializeField] protected int HP, maxHP, stunned, movementLocked;
    public EntityTypes entityID;

    public enum EntityTypes { Enemy, Player, None }

    // Start is called before the first frame update
    public void TakeDamage(int amount, EntityTypes ignoreEntity)
    {
        if (ignoreEntity == entityID) { return; }

        HP -= amount;

        if (HP < 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
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
}
