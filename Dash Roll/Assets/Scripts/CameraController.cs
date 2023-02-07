using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTrfm, trackingTrfm, targetTrfm;
    public float trackingRate, rotationRate, moveIntensity, rotationIntensity;

    public static CameraController self;

    [SerializeField] int mode;
    public const int TRACK_PLAYER = 0, PANNING = 1, CAMERA_STOP = 2, ELEVATOR = 3;

    Vector3 cameraTrackingVect3;

    // Start is called before the first frame update
    void Start()
    {
        trackingTrfm.parent = null;
        self = GetComponent<CameraController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (mode == TRACK_PLAYER)
        {
            cameraTrackingVect3.x = (targetTrfm.position.x - trackingTrfm.position.x) * trackingRate;
            cameraTrackingVect3.y = (targetTrfm.position.y - trackingTrfm.position.y) * trackingRate;
        }
        else if (mode == PANNING)
        {
            cameraTrackingVect3.x = (panPositions[currentPanIndex].x - trackingTrfm.position.x) * trackingRate;
            cameraTrackingVect3.y = (panPositions[currentPanIndex].y - trackingTrfm.position.y) * trackingRate;
        }
        else if (mode == CAMERA_STOP)
        {
            cameraTrackingVect3.x = (lastTargetPos.x - trackingTrfm.position.x) * trackingRate;
            cameraTrackingVect3.y = (lastTargetPos.y - trackingTrfm.position.y) * trackingRate;
        } else if (mode == ELEVATOR)
        {
            //trackingTrfm.position = new Vector3(trackingTrfm.position.x, ElevatorManagerIII.trfm.position.y + 3, -10);

            cameraTrackingVect3.x = (targetTrfm.position.x - trackingTrfm.position.x) * trackingRate;

            cameraTrackingVect3.y = (ElevatorManagerIII.trfm.position.y - 16 - trackingTrfm.position.y) * trackingRate;
            float defaultYPos = (targetTrfm.position.y - trackingTrfm.position.y) * trackingRate;

            if (cameraTrackingVect3.y < defaultYPos)
            {
                cameraTrackingVect3.y = defaultYPos;
            }
        }

        trackingTrfm.position += cameraTrackingVect3;

        ProcessTrauma();
        ProcessSleep();
        ProcessPanning();
    }

    public void SetMode(int p_mode, bool overrideElevatorMode = false)
    {
        if (mode != ELEVATOR || overrideElevatorMode)
        {
            Debug.Log("new mode: " + p_mode);
            mode = p_mode;
        }
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

    float processedTrauma;
    Vector3 zVect3;
    void ProcessTrauma()
    {
        //rotational "recovery"
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

        //translational "recovery" (lerps rotation back to level)
        cameraTrackingVect3.x = (trackingTrfm.position.x - cameraTrfm.position.x) * trackingRate;
        cameraTrackingVect3.y = (trackingTrfm.position.y - cameraTrfm.position.y) * trackingRate;

        cameraTrfm.position += cameraTrackingVect3;

        //screen shake/rotation
        if (trauma > 0)
        {
            if (trauma > 48) //hard cap trauma at 40
            {
                processedTrauma = 2100;
            }
            else if (trauma > 30) //soft cap at 30 trauma
            {
                processedTrauma = 900 + 60 * (trauma - 30);
            }
            else
            {
                //amount of "shake" is proportional to trauma squared
                processedTrauma = trauma * trauma;
            }

            //generate random Translational offset for camera per tick
            cameraTrackingVect3 = Random.insideUnitCircle.normalized * moveIntensity * processedTrauma;
            cameraTrfm.position += cameraTrackingVect3;

            //generate random Rotational offset for camera per tick
            cameraTrfm.Rotate(Vector3.forward * rotationIntensity * (Random.Range(0,2) * 2 - 1) * processedTrauma);

            //decrement trauma as a timer
            trauma--;
        }
    }

    int sleepTimer;
    public static void Sleep(int amount)
    {
        if (amount < 1) { return; }
        if (self.sleepTimer < 1) { Time.timeScale = .4f; }
        if (self.sleepTimer < amount) { self.sleepTimer = amount; }
    }

    void ProcessSleep()
    {
        if (sleepTimer > 0)
        {
            if (sleepTimer == 1) { Time.timeScale = 1; }
            sleepTimer--;
        }
    }

    [SerializeField] Vector2[] panPositions = new Vector2[3];
    [SerializeField] int[] panDurations = new int[3], panPriorities = new int[3];
    [SerializeField] int currentPanIndex, nextAvailablePanIndex;

    public static void QueCameraPan(Vector2 position, int duration, int priority)
    {
        self.m_QueCameraPan(position, duration, priority);
    }

    void m_QueCameraPan(Vector2 position, int duration, int priority)
    {
        for (int i = 0; i < 3; i++)
        {
            if (panDurations[i] < 1)
            {
                panPositions[i] = position;
                panDurations[i] = duration;
                panPriorities[i] = priority;

                if (panDurations[currentPanIndex] < 1 || priority > panPriorities[currentPanIndex]) { currentPanIndex = i; }

                SetMode(PANNING);

                return;
            }
        }

        Debug.Log("camera pan que full!");
    }

    void ProcessPanning()
    {
        if (panDurations[currentPanIndex] > 0)
        {
            panDurations[currentPanIndex]--;

            if (panDurations[currentPanIndex] < 1)
            {
                currentPanIndex = -1;
                for (int i = 0; i < panPositions.Length; i++)
                {
                    if (currentPanIndex == -1)
                    {
                        if (panDurations[i] > 0) { currentPanIndex = i; }
                    }
                    else if (panPriorities[i] > panPriorities[currentPanIndex])
                    {
                        currentPanIndex = i;
                    }
                }
                if (currentPanIndex == -1) 
                {
                    SetMode(TRACK_PLAYER);
                    currentPanIndex = 0; 
                }
            }
        }
    }

    Vector3 lastTargetPos;
    public static void StopCameraTracking()
    {
        self.SetMode(CAMERA_STOP);
        self.lastTargetPos = self.targetTrfm.position;
    }
}
