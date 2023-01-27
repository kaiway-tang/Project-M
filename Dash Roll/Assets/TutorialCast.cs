using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCast : MonoBehaviour
{
    bool firstFired;
    private void FixedUpdate()
    {

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!firstFired)
        {
            if (PlayerInput.CastHeld()) { firstFired = true; }
            if (col.gameObject.layer == 1) { Player.AddMana(100); }
        }
    }
}
