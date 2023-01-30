using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] ObjectPooler
        telegraphPooler, slashFXPooler, kickRingFXPooler, bloodFXPooler, bloodFXLargePooler, hitRingFXPooler, jumpRingFXPooler,
        healthShardPooler;

    [SerializeField] Material defaultMaterial, flashMaterial;
    [SerializeField] GameInitializer self;
    [SerializeField] Transform headTrfm;
    static bool gameInitialized;
    // Start is called before the first frame update
    void Awake()
    {
        //if (gameInitialized) { return; }
            
        Enemy.telegraphPooler = telegraphPooler;
        Enemy.defaultMaterial = defaultMaterial;
        Enemy.flashMaterial = flashMaterial;
        Enemy.kickRingFXPooler = kickRingFXPooler;
        Enemy.bloodFXLargePooler = bloodFXLargePooler;
        Enemy.healthShardPooler = healthShardPooler;

        SlashAttack.slashFXPooler = slashFXPooler;
        SlashAttack.hitRingFXPooler = hitRingFXPooler;
        KickAttack.kickRingFXPooler = kickRingFXPooler;
        ShootingSword.slashFXPooler = slashFXPooler;
        ShootingSword.hitRingFXPooler = hitRingFXPooler;
        Obelisk.healthShardPooler = healthShardPooler;

        HPEntity.bloodFXPooler = bloodFXPooler;

        Player.headTrfm = headTrfm;
        Player.jumpRingFXPooler = jumpRingFXPooler;

        gameInitialized = true;
        Destroy(self);
    }
}
