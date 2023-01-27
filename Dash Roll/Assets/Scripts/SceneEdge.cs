using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEdge : CameraStop
{
    [SerializeField] int nextScene;
    [SerializeField] Vector3 spawnPosition;

    new void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 1)
        {
            TriggerEnteredByPlayer();
            Player.nextScene = nextScene;
            GameManager.spawnPosition = spawnPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 1)
        {
            TriggerExitedByPlayer();
            Player.nextScene = -1;
        }
    }
}
