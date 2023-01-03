using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : DirectionalAttack
{
    public int traumaOnHit;
    public static ObjectPooler slashFXPooler;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected new void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        if (takeDamageResult != HPEntity.IGNORED)
        {
            slashFXPooler.Instantiate(col.transform.position, Random.Range(-45, 46));

            if (entityType == HPEntity.EntityTypes.Player)
            {
                CameraController.SetTrauma(traumaOnHit);
                CameraController.Sleep(1);
            }
        }
    }
}
