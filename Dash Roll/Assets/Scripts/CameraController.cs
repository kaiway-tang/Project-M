using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform cameraTrfm, trackingTrfm, targetTrfm;
    [SerializeField] float trackingRate, rotationRate, moveIntensity, rotationIntensity;

    static CameraController self;

    int mode;
    const int TRACK_PLAYER = 0;

    Vector3 vect3;

    // Start is called before the first frame update
    void Start()
    {
        trackingTrfm.parent = null;
        self = GetComponent<CameraController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) trauma += 10;
        if (Input.GetKeyDown(KeyCode.Alpha2)) trauma += 20;
        if (Input.GetKeyDown(KeyCode.Alpha3)) trauma += 30;
        if (Input.GetKeyDown(KeyCode.Alpha4)) trauma += 40;
        if (Input.GetKeyDown(KeyCode.Alpha5)) trauma += 50;
        if (Input.GetKeyDown(KeyCode.Alpha6)) trauma += 60;
        if (Input.GetKeyDown(KeyCode.Alpha7)) trauma += 70;
        if (Input.GetKeyDown(KeyCode.Alpha8)) trauma += 80;
        if (Input.GetKeyDown(KeyCode.Alpha9)) trauma += 90;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mode == TRACK_PLAYER)
        {
            vect3.x = (targetTrfm.position.x - trackingTrfm.position.x) * trackingRate;
            vect3.y = (targetTrfm.position.y - trackingTrfm.position.y) * trackingRate;

            trackingTrfm.position += vect3;
        }

        processTrauma();
    }

    [SerializeField] int trauma;
    public static void AddTrauma(int amount, int max = int.MaxValue)
    {
        self.trauma += amount;
        if (self.trauma > max)
        {
            self.trauma = max;
        }
    }
    public static void SetTrauma(int amount)
    {
        if (self.trauma < amount)
        {
            self.trauma = amount;
        }
    }

    [SerializeField] float processedTrauma;
    Vector3 zVect3;
    void processTrauma()
    {
        //rotational recovery
        if (cameraTrfm.localEulerAngles.z < .1f || cameraTrfm.localEulerAngles.z > 359.9f) 
        {
            cameraTrfm.localEulerAngles = Vector3.zero; 
        }
        else
        {
            if (cameraTrfm.localEulerAngles.z < 180) { zVect3.z = -cameraTrfm.localEulerAngles.z * rotationRate; }
            else { zVect3.z = (360 - cameraTrfm.localEulerAngles.z) * rotationRate; }
            cameraTrfm.localEulerAngles += zVect3;
        }

        //translational recovery
        vect3.x = (trackingTrfm.position.x - cameraTrfm.position.x) * trackingRate;
        vect3.y = (trackingTrfm.position.y - cameraTrfm.position.y) * trackingRate;

        cameraTrfm.position += vect3;

        //screen shake/rotation
        if (trauma > 0)
        {
            if (trauma > 48)
            {
                processedTrauma = 2100;
            }
            else if (trauma > 30)
            {
                processedTrauma = 900 + 60 * (trauma - 30);
            }
            else
            {
                processedTrauma = trauma * trauma;
            }

            vect3 = Random.insideUnitCircle.normalized * moveIntensity * processedTrauma;
            cameraTrfm.position += vect3;

            cameraTrfm.Rotate(Vector3.forward * rotationIntensity * (Random.Range(0,2) * 2 - 1) * processedTrauma);

            trauma--;
        }
    }


}
