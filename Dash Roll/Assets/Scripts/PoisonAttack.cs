using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttack : DirectionalAttack
{
    [SerializeField] int poison;

    private new void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);

        if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            col.GetComponent<HPEntity>().Poison(poison, entityType);
        }
    }
}
