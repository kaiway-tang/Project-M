using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    static Color fade = new Color(0,0,0,.02f);
    bool fading;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fading)
        {
            spriteRenderer.color -= fade;
            if (spriteRenderer.color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        fading = true;
    }
}
