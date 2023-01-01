using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Material flash, normal;

    bool everyTwo;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.material = flash;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void EveryTwo()
    {
        
    }
}
