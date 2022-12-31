using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected int damage, knockback;
    [SerializeField] protected float speed;
    [SerializeField] protected HPEntity.EntityTypes entityType;
    [SerializeField] protected Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.position += trfm.up * speed;
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<MobileEntity>().TakeDamage(damage, entityType, trfm.up * knockback);
        Destroy(gameObject);
    }
}