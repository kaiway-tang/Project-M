using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprites;
    [SerializeField] int[] time;
    int timer, currentSprite;
    public void Play()
    {
        timer = 0;
        currentSprite = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer > -1)
        {
            if (timer == time[currentSprite])
            {
                if (currentSprite >= sprites.Length)
                {
                    spriteRenderer.sprite = null;
                    timer = -1;
                    return;
                }

                spriteRenderer.sprite = sprites[currentSprite];
                currentSprite++;

                if (currentSprite >= time.Length)
                {
                    timer = -1;
                    return;
                }
            }
            timer++;
        }
    }
}
