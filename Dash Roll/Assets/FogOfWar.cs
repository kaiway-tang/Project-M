using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject[] triggerObjects;
    [SerializeField] bool fading, objectTriggered;
    static Color fade = new Color(0,0,0,.02f);
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
        if (objectTriggered)
        {
            for (int i = 0; i < triggerObjects.Length; i++)
            {
                if (triggerObjects[i])
                {
                    break;
                }

                if (i == triggerObjects.Length - 1)
                {
                    TriggerFade();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        TriggerFade();
    }

    public void TriggerFade()
    {
        fading = true;
    }
}
