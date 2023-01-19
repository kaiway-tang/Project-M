using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : CameraStop
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 1)
        {
            TriggerEnteredByPlayer();
            CameraController.StopCameraTracking();
            Player.playerScript.isInVoid = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 1 && !Player.playerScript.reviving)
        {
            TriggerExitedByPlayer();
            Player.playerScript.isInVoid = false;
        }
    }
}
