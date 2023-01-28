using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessTeleporter : MonoBehaviour
{
    [SerializeField] Vector3 add;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 1)
        {
            CameraController.self.cameraTrfm.position += add;
            Player.trfm.position += add;
        }
    }
}
