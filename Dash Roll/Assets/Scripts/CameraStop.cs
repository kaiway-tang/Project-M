using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStop : MonoBehaviour
{
    protected void Start()
    {
        Destroy(GetComponent<SpriteRenderer>());
    }

    protected void TriggerEnteredByPlayer()
    {
        CameraController.StopCameraTracking();
    }

    protected void TriggerExitedByPlayer()
    {
        CameraController.self.mode = CameraController.TRACK_PLAYER;
    }
}
