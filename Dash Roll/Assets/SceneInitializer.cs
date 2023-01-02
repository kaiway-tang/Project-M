using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] ObjectPooler telegraphPooler, slashFXPooler, ringFXPooler, bloodFXPooler;
    [SerializeField] Material defaultMaterial, flashMaterial;
    [SerializeField] SceneInitializer self;
    // Start is called before the first frame update
    void Awake()
    {
        Enemy.telegraphPooler = telegraphPooler;
        Enemy.defaultMaterial = defaultMaterial;
        Enemy.flashMaterial = flashMaterial;

        SlashAttack.slashFXPooler = slashFXPooler;
        KickAttack.ringFXPooler = ringFXPooler;

        HPEntity.bloodFXPooler = bloodFXPooler;

        Destroy(self);
    }
}
