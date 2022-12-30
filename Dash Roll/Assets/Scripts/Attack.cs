using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected HPEntity.EntityTypes entityType;
    [SerializeField] protected Transform trfm;
    [SerializeField] Collider2D collider;
    protected MobileEntity mobileEntity;
    
    public void Activate()
    {
        collider.enabled = true;
    }

    public void Deactivate()
    {
        collider.enabled = false;
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<HPEntity>().TakeDamage(damage, entityType);
    }
}
