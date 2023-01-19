using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiClip : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.layer == 7)
        {
            Debug.Log("stuck in terrain!");
            Player.trfm.position += Vector3.up * .9f;
        }
    }
}
