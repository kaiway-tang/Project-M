using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] Transform hpTrfm, highlightTrfm;
    [SerializeField] SpriteRenderer[] spriteRenderer;
    static Color fadeColor = new Color(0, 0, 0, .1f), startColor = new Color(0,0,0,1);
    static Vector3 setPercentageScale = Vector3.one, highlightShrinkScale = Vector3.zero;
    bool animateBar;
    int fadeTimer;

    public void SetPercentage(float percentage) //1.0 = 100% (full hp)
    {
        setPercentageScale.x = percentage;
        hpTrfm.localScale = setPercentageScale;
        animateBar = true;
        fadeTimer = 0;

        for (int i = 0; i < 3; i++)
        {
            startColor = spriteRenderer[i].color;
            startColor.a = 1;
            spriteRenderer[i].color = startColor;
        }
    }
    
    protected void FixedUpdate()
    {
        if (animateBar)
        {
            if (fadeTimer > 0)
            {
                if (fadeTimer == 1)
                {
                    if (spriteRenderer[0].color.a > 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            spriteRenderer[i].color -= fadeColor;
                        }
                    }
                    else
                    {
                        animateBar = false;
                    }
                }
                else
                {
                    fadeTimer--;
                }
            }
            else
            {
                if (highlightTrfm.localScale.x - hpTrfm.localScale.x > .02f)
                {
                    highlightShrinkScale.x = (hpTrfm.localScale.x - highlightTrfm.localScale.x) * .1f;
                    highlightTrfm.localScale += highlightShrinkScale;
                }
                else
                {
                    setPercentageScale.x = hpTrfm.localScale.x;
                    highlightTrfm.localScale = setPercentageScale;
                    fadeTimer = 50;
                }
            }
        }
    }
}
