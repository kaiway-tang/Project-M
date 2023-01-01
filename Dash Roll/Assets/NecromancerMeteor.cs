using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerMeteor : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] int timer;
    [SerializeField] Vector3 move;
    [SerializeField] Transform trfm;
    // Start is called before the first frame update
    void Start()
    {
        if (trfm.position.x > PlayerMovement.trfm.position.x) { move.x *= -1; }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.position += move;
        timer--;
        if (timer < 1)
        {
            Instantiate(explosion, trfm.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
