using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 1 && col.GetComponent<PlayerInput>())
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
