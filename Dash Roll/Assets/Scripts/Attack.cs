using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] int damage, knockback;
    [SerializeField] HPEntity.EntityTypes EntityTypes;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<HPEntity>().TakeDamage(damage, EntityTypes);
    }
}
