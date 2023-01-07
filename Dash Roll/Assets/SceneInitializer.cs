using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] ObjectPooler telegraphPooler, slashFXPooler, kickRingFXPooler, bloodFXPooler, hitRingFXPooler, jumpRingFXPooler;
    [SerializeField] Material defaultMaterial, flashMaterial;
    [SerializeField] SceneInitializer self;
    [SerializeField] Transform headTrfm;
    // Start is called before the first frame update
    void Awake()
    {
        Enemy.telegraphPooler = telegraphPooler;
        Enemy.defaultMaterial = defaultMaterial;
        Enemy.flashMaterial = flashMaterial;
        Enemy.kickRingFXPooler = kickRingFXPooler;

        SlashAttack.slashFXPooler = slashFXPooler;
        SlashAttack.hitRingFXPooler = hitRingFXPooler;
        KickAttack.kickRingFXPooler = kickRingFXPooler;
        ShootingSword.slashFXPooler = slashFXPooler;
        ShootingSword.hitRingFXPooler = hitRingFXPooler;

        HPEntity.bloodFXPooler = bloodFXPooler;

        PlayerMovement.headTrfm = headTrfm;
        PlayerMovement.jumpRingFXPooler = jumpRingFXPooler;

        Destroy(self);
    }
}
