using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : DirectionalAttack
{
    public int traumaOnHit;
    public static ObjectPooler slashFXPooler, hitRingFXPooler;
    [SerializeField] HPEntity.DamageSource damageSource;
    HPEntity colEntity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected new void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            colEntity = col.GetComponent<HPEntity>();
            takeDamageResult = colEntity.TakeDamage(damage, entityType, knockbackDirections[direction], damageSource);
            if (takeDamageResult != HPEntity.IGNORED)
            {
                if (!manaGranted && colEntity.grantsMana)
                {
                    Player.AddMana(damage, damage); manaGranted = true;
                }

                slashFXPooler.Instantiate(colEntity.trfm.position, Random.Range(-45, 46));
                hitRingFXPooler.Instantiate(colEntity.trfm.position, 0);

                if (entityType == HPEntity.EntityTypes.Player)
                {
                    CameraController.SetTrauma(traumaOnHit);
                    CameraController.Sleep(1);
                }
            }
        }
    }
}
