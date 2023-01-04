using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] ObjectPooler telegraphPooler, slashFXPooler, kickRingFXPooler, bloodFXPooler, hitRingFXPooler;
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

        HPEntity.bloodFXPooler = bloodFXPooler;

        PlayerMovement.headTrfm = headTrfm;

        Destroy(self);
    }
}
