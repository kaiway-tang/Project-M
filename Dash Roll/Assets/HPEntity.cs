using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPEntity : MonoBehaviour
{
    [SerializeField] protected int HP, maxHP, stunned, movementLocked;
    public int entityID;
    const int enemyID = 0, playerID = 1;
    // Start is called before the first frame update
    protected void TakeDamage(int amount, int ignoreID = -1)
    {
        HP -= amount;

        if (HP < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void Heal(int amount, bool allowOverheal)
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
