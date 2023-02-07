using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallElevator : MonoBehaviour
{
    [SerializeField] float riseRate, top;
    [SerializeField] bool rising;
    [SerializeField] Transform elevatorTrfm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rising)
        {
            elevatorTrfm.position += Vector3.up * riseRate;
            if (elevatorTrfm.position.y > top)
            {
                elevatorTrfm.position += Vector3.up * (top - elevatorTrfm.position.y);
                CameraController.SetTrauma(17);
                rising = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 1 && col.GetComponent<PlayerInput>())
        {
            Player.trfm.parent = elevatorTrfm;
            rising = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 1 && col.GetComponent<PlayerInput>())
        {
            Player.trfm.parent = null;
        }
    }
}
