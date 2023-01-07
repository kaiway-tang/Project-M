using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalHazard : MonoBehaviour
{
    [SerializeField] int damage, knockback;
    [SerializeField] Vector3 knockbackSourceOffset;
    int takeDamageResult;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            takeDamageResult = col.GetComponent<HPEntity>().TakeDamage(damage, HPEntity.EntityTypes.Neutral, transform.position + knockbackSourceOffset, knockback);
            return;
        }
        takeDamageResult = HPEntity.IGNORED;
    }
}
