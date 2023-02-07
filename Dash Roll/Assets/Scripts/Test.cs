using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Transform trfm;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector3 move;
    void Start()
    {
    }

    void FixedUpdate()
    {
        rb.velocity = move;
    }
}
