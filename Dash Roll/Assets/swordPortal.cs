using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordPortal : MonoBehaviour
{
    [SerializeField] GameObject swordObj;
    [SerializeField] Transform trfm;
    [SerializeField] Vector3 scale;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color fade;
    [SerializeField] SpriteMask mask;
    [SerializeField] ParticleSystem swordSpawnPtcls;
    int timer;
    // Start is called before the first frame update
    void Start()
    {
        trfm.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer < 10)
        {
            trfm.localScale += scale;
        }
        else
        {
            spriteRenderer.color -= fade * .5f;

            if (timer == 10)
            {
                CameraController.SetTrauma(18);
                swordSpawnPtcls.Play();
                Instantiate(swordObj, trfm.position, trfm.rotation);
            }
            if (timer == 12)
            {
                mask.enabled = false;
            }

            if (timer > 40)
            {
                Destroy(gameObject);
            }

            /*
            if (timer > 20)
            {
                trfm.localScale -= scale;
                if (timer > 25)
                {
                    Destroy(gameObject);
                }
            }
            */
        }

        timer++;
    }
}
