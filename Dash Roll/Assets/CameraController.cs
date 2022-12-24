using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform trfm;
    [SerializeField] Transform plyrTrfm;
    [SerializeField] float trackingRate;

    int mode;
    const int TRACK_PLAYER = 0;

    Vector3 vect3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mode == TRACK_PLAYER)
        {
            vect3.x = (plyrTrfm.position.x - trfm.position.x) * trackingRate;
            vect3.y = (plyrTrfm.position.y - trfm.position.y) * trackingRate;

            trfm.position += vect3;
        }
    }
}
