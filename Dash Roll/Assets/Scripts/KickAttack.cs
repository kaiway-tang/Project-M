using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickAttack : DirectionalAttack
{
    [SerializeField] PlayerMovement playerMovement;
    public static ObjectPooler ringFXPooler;
    new void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 12)
        {
            if (col.GetComponent<MobileEntity>().TakeDamage(damage, entityType, knockbackDirections[direction]) == HPEntity.ALIVE)
            {
                col.GetComponent<Enemy>().kicked = 35;
                SuccessfulHit();
            }
        } else
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            if (col.GetComponent<MobileEntity>().TakeDamage(damage, entityType, knockbackDirections[direction]) == HPEntity.ALIVE)
            {
                SuccessfulHit();
            }
        }
    }

    void SuccessfulHit()
    {
        playerMovement.TakeKnockback(knockbackDirections[direction] * -.8f);
        ringFXPooler.Instantiate(trfm.position, 90);
        CameraController.SetTrauma(16);
        CameraController.Sleep(3);
    }
}
