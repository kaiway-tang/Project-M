using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidKill : MonoBehaviour
{
    bool started;
    private void Start()
    {
        started = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!started) { return; }
        if (col.gameObject.layer == 11)
        {
            Player.playerScript.Kill();
        }
        else if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            col.GetComponent<HPEntity>().TakeDamage(999, HPEntity.EntityTypes.Player);
        }

    }
}
