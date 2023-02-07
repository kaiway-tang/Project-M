using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Transform[] indicatorTrfms;
    [SerializeField] SpriteRenderer pulseSpriteRenderer;
    [SerializeField] GameObject laserObj;
    static Color fade;
    [SerializeField] int fireDuration;
    int firingTimer;
    bool strobeFading;
    public bool active;

    Vector3 vect3;
    static Vector3 indicatorTrfmshrinkRate;
    // Start is called before the first frame update
    void Start()
    {
        fade = new Color(0, 0, 0, 0.33f);
        indicatorTrfmshrinkRate = new Vector3(0, 0.02f, 0);
    }

    void FixedUpdate()
    {
        if (firingTimer > 0)
        {
            if (firingTimer > fireDuration)
            {
                indicatorTrfms[0].localPosition -= indicatorTrfmshrinkRate;
                indicatorTrfms[1].localPosition += indicatorTrfmshrinkRate;
            }
            else if (firingTimer == fireDuration)
            {
                indicatorTrfms[0].gameObject.SetActive(false);
                indicatorTrfms[1].gameObject.SetActive(false);

                laserObj.SetActive(true);
                pulseSpriteRenderer.gameObject.SetActive(true);
            } else
            {
                if (strobeFading)
                {
                    pulseSpriteRenderer.color -= fade;
                }
                else
                {
                    pulseSpriteRenderer.color += fade;
                }

                if (firingTimer % 3 == 0)
                {
                    strobeFading = !strobeFading;
                }

                if (firingTimer == 1)
                {
                    pulseSpriteRenderer.gameObject.SetActive(false);
                    laserObj.SetActive(false);
                }
            }

            firingTimer--;
        }
    }

    public void Fire()
    {
        indicatorTrfms[0].gameObject.SetActive(true);
        indicatorTrfms[1].gameObject.SetActive(true);

        vect3.x = 0; vect3.z = 0;
        vect3.y = .4f;

        indicatorTrfms[0].localPosition = vect3;
        vect3.y = -.4f;
        indicatorTrfms[1].localPosition = vect3;

        pulseSpriteRenderer.color = Color.white;
        strobeFading = true;

        firingTimer = fireDuration + 20;
    }
}
