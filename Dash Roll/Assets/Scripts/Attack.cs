using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected HPEntity.EntityTypes entityType;
    public Transform trfm;
    [SerializeField] Collider2D hitbox;

    protected bool manaGranted;

    protected int takeDamageResult;
    
    public void Activate()
    {
        hitbox.enabled = true;
        manaGranted = false;
    }

    public void Deactivate()
    {
        hitbox.enabled = false;
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14) 
        {
            //if (!manaGranted) { Player.AddMana(10, col.GetComponent<HPEntity>().GetHP()); }
            takeDamageResult = col.GetComponent<HPEntity>().TakeDamage(damage, entityType);
            return;
        }
        takeDamageResult = HPEntity.IGNORED;
    }
}
