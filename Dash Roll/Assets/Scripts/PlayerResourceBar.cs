using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceBar : HPBar
{
    [SerializeField] bool animateHighlight;

    public override void SetPercentage(float percentage) //1.0 = 100% (full hp)
    {
        setPercentageScale.x = percentage;
        hpTrfm.localScale = setPercentageScale;
        animateBar = animateHighlight;
    }

    protected new void FixedUpdate()
    {
        if (animateBar)
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
                animateBar = false;
            }
        }
    }
}
