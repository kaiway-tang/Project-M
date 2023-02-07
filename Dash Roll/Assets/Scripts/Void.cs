using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : CameraStop
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 1 && col.GetComponent<PlayerInput>())
        {
            TriggerEnteredByPlayer();
            CameraController.StopCameraTracking();
            Player.playerScript.isInVoid = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 1 && col.GetComponent<PlayerInput>() && !Player.playerScript.reviving)
        {
            TriggerExitedByPlayer();
            Player.playerScript.isInVoid = false;
        }
    }
}
