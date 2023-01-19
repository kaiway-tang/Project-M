using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPoint : MonoBehaviour
{
    Vector3 position;
    void Start()
    {
        position = transform.position;
        position.y -= 4;
        Destroy(GetComponent<SpriteRenderer>());
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 1)
        {
            Player.lastSafePosition = position;
        }
    }
}
