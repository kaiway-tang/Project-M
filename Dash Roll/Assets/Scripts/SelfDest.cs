using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDest : MonoBehaviour
{
    [SerializeField] int life;

    protected void FixedUpdate()
    {
        life--;
        if (life < 1)
        {
            Destroy(gameObject);
        }
    }
}
