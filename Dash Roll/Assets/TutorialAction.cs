using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAction : MonoBehaviour
{
    int timer;

    protected void FixedUpdate()
    {
        if (timer > 0)
        {
            if (timer == 5)
            {
                if (Time.timeScale > .15f)
                {
                    Time.timeScale -= .05f;
                }
                timer = 1;
            }
            timer++;
        }
    }

    protected void EnterSloMo(float timeScale)
    {
        timer = 1;
        Time.timeScale = timeScale;
    }

    protected void ExitSloMo()
    {
        Time.timeScale = 1;
        timer = 0;
    }

}
