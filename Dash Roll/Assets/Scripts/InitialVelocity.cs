using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialVelocity : MonoBehaviour
{
    [SerializeField] Rigidbody2D[] rb;
    [SerializeField] Vector2[] velocities;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rb.Length; i++)
        {
            rb[i].velocity = velocities[i];
        }
        Destroy(this);
    }
}
