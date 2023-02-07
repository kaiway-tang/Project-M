using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManagerII : MonoBehaviour
{
    [SerializeField] GameObject[] chunks;
    [SerializeField] Transform[] backgroundTrfms;
    [SerializeField] Parallaxer parallaxer;
    [SerializeField] Vector3 riseRate;
    public bool dismounted;
    [SerializeField] float altitude;

    public static ElevatorManagerII self;
    Transform trfm;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<ElevatorManagerII>();
        trfm = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dismounted)
        {
            for (int i = 0; i < backgroundTrfms.Length; i++)
            {
                backgroundTrfms[i].position += riseRate;
            }

            parallaxer.referencePoint.y -= riseRate.y * .025f;
        }
        else
        {
            trfm.position -= riseRate;
        }
    }

    public void Dismount()
    {
        dismounted = true;
    }

    public void Remount()
    {
        dismounted = false;
    }
}
