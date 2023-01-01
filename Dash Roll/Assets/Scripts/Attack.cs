using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected HPEntity.EntityTypes entityType;
    [SerializeField] protected Transform trfm;
    [SerializeField] Collider2D hitbox;
    
    public void Activate()
    {
        hitbox.enabled = true;
    }

    public void Deactivate()
    {
        hitbox.enabled = false;
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14) { col.GetComponent<HPEntity>().TakeDamage(damage, entityType); }
    }
}
