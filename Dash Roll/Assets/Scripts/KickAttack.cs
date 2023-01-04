using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickAttack : DirectionalAttack
{
    [SerializeField] PlayerMovement playerMovement;
    public static ObjectPooler kickRingFXPooler;
    new void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {
            SuccessfulHit();
            if (col.GetComponent<MobileEntity>().TakeDamage(damage, entityType, knockbackDirections[direction]) == HPEntity.ALIVE)
            {
                playerMovement.TakeKnockback(knockbackDirections[direction] * -.8f);
                col.GetComponent<Enemy>().kicked = 50;
            }
        } else
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            SuccessfulHit();
            if (col.GetComponent<MobileEntity>().TakeDamage(damage, entityType, knockbackDirections[direction]) == HPEntity.ALIVE)
            {
                playerMovement.TakeKnockback(knockbackDirections[direction] * -.8f);
                SuccessfulHit();
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
