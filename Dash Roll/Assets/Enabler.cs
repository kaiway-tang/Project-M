using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enabler : MonoBehaviour
{
    [SerializeField] GameObject[] enable;
    [SerializeField] GameObject[] disable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 1)
        {
            for (int i = 0; i < enable.Length; i++)
            {
                enable[i].SetActive(true);
            }
            for (int i = 0; i < disable.Length; i++)
            {
                disable[i].SetActive(false);
            }
        } 
    }
}