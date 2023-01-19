using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickAttack : DirectionalAttack
{
    [SerializeField] Player playerScript;
    public static ObjectPooler kickRingFXPooler;
    [SerializeField] HPEntity.DamageSource damageSource;
    new void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {
            Player.AddMana(damage, col.GetComponent<HPEntity>().GetHP());
            SuccessfulHit();
            if (col.GetComponent<HPEntity>().TakeDamage(damage, entityType, knockbackDirections[direction], damageSource) == HPEntity.ALIVE)
            {
                playerScript.TakeKnockback(knockbackDirections[direction] * -.8f);
                col.GetComponent<Enemy>().kicked = 50;
            }
        } else
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            Player.AddMana(damage, col.GetComponent<HPEntity>().GetHP());
            SuccessfulHit();
            if (col.GetComponent<HPEntity>().TakeDamage(damage, entityType, knockbackDirections[direction], damageSource) == HPEntity.ALIVE)
            {
                playerScript.TakeKnockback(knockbackDirections[direction] * -.8f);
            }
        }
    }

    void SuccessfulHit()
    {
        kickRingFXPooler.Instantiate(trfm.position, 90);
        CameraController.SetTrauma(16);
        CameraController.Sleep(1);
    }
}
