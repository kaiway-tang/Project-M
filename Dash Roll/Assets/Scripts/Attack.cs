using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected HPEntity.EntityTypes entityType;
    [SerializeField] protected Transform trfm;
    protected MobileEntity mobileEntity;
    // Start is called before the first frame update

    protected void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<HPEntity>().TakeDamage(damage, entityType);
    }
}
