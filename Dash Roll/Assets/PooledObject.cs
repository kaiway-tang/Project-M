using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [SerializeField] ObjectPooler objectPooler;
    [SerializeField] int objectID, life;
    int timer;

    private void Start()
    {
        timer = life;
    }

    protected void Destantiate()
    {
        if (objectID == 0)
        {
            Destroy(gameObject);
            return;
        }

        objectPooler.objectReady[objectID - 1] = true;
        gameObject.SetActive(false);
    }
    protected void FixedUpdate()
    {
        if (timer > 0)
        {
            timer--;
            if (timer == 0)
            {
                timer = life;
                Destantiate();
            }
        }
    }
}
