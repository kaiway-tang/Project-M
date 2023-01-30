using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Transform[] indicatorTrfms;
    [SerializeField] SpriteRenderer pulseSpriteRenderer;
    [SerializeField] GameObject laserObj;
    static Color fade;
    [SerializeField] int timer;
    bool strobeFading;
    public bool active;

    Vector3 vect3;
    static Vector3 indicatorTrfmshrinkRate;
    // Start is called before the first frame update
    void Start()
    {
        fade = new Color(0, 0, 0, 0.33f);
        timer = Random.Range(50, 200);
        indicatorTrfmshrinkRate = new Vector3(0.01f, 0, 0);
    }

    void FixedUpdate()
    {
        if (active)
        {
            if (timer > 0)
            {
                if (timer > 480)
                {
                    indicatorTrfms[0].localPosition -= indicatorTrfmshrinkRate;
                    indicatorTrfms[1].localPosition += indicatorTrfmshrinkRate;
                }

                if (timer == 480)
                {
                    indicatorTrfms[0].gameObject.SetActive(false);
                    indicatorTrfms[1].gameObject.SetActive(false);

                    laserObj.SetActive(true);
                    pulseSpriteRenderer.gameObject.SetActive(true);
                }

                if (timer > 466 && timer < 480)
                {
                    if (strobeFading)
                    {
                        pulseSpriteRenderer.color -= fade;
                    }
                    else
                    {
                        pulseSpriteRenderer.color += fade;
                    }

                    if (timer % 3 == 0)
                    {
                        strobeFading = !strobeFading;
                    }
                }

                if (timer == 466)
                {
                    pulseSpriteRenderer.gameObject.SetActive(false);
                    laserObj.SetActive(false);

                    timer -= Random.Range(250, 400);
                }

                timer--;
            }
            else
            {
                indicatorTrfms[0].gameObject.SetActive(true);
                indicatorTrfms[1].gameObject.SetActive(true);

                vect3.y = 60; vect3.z = 0;
                vect3.x = .2f;

                indicatorTrfms[0].localPosition = vect3;
                vect3.x = -.2f;
                indicatorTrfms[1].localPosition = vect3;

                pulseSpriteRenderer.color = Color.white;
                strobeFading = true;

                timer = 500;
            }
        }
    }
}
