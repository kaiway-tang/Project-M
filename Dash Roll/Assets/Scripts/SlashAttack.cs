using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : DirectionalAttack
{
    public int traumaOnHit;
    public static ObjectPooler slashFXPooler, hitRingFXPooler;
    [SerializeField] HPEntity.DamageSource damageSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected new void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            if (col.GetComponent<HPEntity>().TakeDamage(damage, entityType, knockbackDirections[direction], damageSource) != HPEntity.IGNORED)
            {
                if (!manaGranted) { Player.AddMana(damage); manaGranted = true; }
                slashFXPooler.Instantiate(col.transform.position, Random.Range(-45, 46));
                hitRingFXPooler.Instantiate(col.transform.position, 0);

                if (entityType == HPEntity.EntityTypes.Player)
                {
                    CameraController.SetTrauma(traumaOnHit);
                    CameraController.Sleep(1);
                }
            }
        }
    }
}
