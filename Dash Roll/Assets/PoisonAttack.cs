using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAttack : DirectionalAttack
{
    [SerializeField] int poison;

    private new void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);

        col.GetComponent<HPEntity>().Poison(poison, entityType);
    }
}
