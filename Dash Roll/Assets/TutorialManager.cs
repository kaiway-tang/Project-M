using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static bool usingWASD, usingArrows;
    [SerializeField] int wasdTracker, arrowsTracker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (usingWASD == usingArrows)
        {
            if (Input.GetKey(PlayerInput.RightKey))
            {
                wasdTracker++;
                if (wasdTracker > 100) { usingWASD = true; }
            }
            if (Input.GetKey(PlayerInput.RightKey1))
            {
                arrowsTracker++;
                if (arrowsTracker > 100) { usingArrows = true; }
            }
        }
    }
}
