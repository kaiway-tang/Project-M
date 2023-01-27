using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPBar : PlayerResourceBar
{
    [SerializeField] ParticleSystem healFX;
    float healFXTimer;
    
    void Start()
    {
        
    }

    public override void SetPercentage(float percentage)
    {
        healFXTimer = hpTrfm.localScale.x - .07f;
        healFX.Play();

        base.SetPercentage(percentage);
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (healFXTimer < hpTrfm.localScale.x)
        {
            healFXTimer += .01f;
        }
        else if (healFX.isPlaying)
        {
            healFX.Stop();
        }
    }
}
