using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform cameraTrfm, trackingTrfm, targetTrfm, swordHUDTrfm;
    public float trackingRate, rotationRate, moveIntensity, rotationIntensity;

    public static CameraController self;

    int mode;
    const int TRACK_PLAYER = 0;

    Vector3 cameraTrackingVect3;

    [SerializeField] SpriteRenderer vignetteRenderer, blackCoverSpriteRenderer;
    float blackCoverTargetAlpha;
    Color fadeRate = new Color(0,0,0,0.01f);
    Color color;

    int alignHUDTimer;

    // Start is called before the first frame update
    void Start()
    {
        trackingTrfm.parent = null;
        self = GetComponent<CameraController>();

        CalculateScreenSize();
        AlignHUDElements();
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
            cameraTrackingVect3.x = (targetTrfm.position.x - trackingTrfm.position.x) * trackingRate;
            cameraTrackingVect3.y = (targetTrfm.position.y - trackingTrfm.position.y) * trackingRate;

            trackingTrfm.position += cameraTrackingVect3;
        }

        ProcessTrauma();
        ProcessSleep();
        ProcessFading();
        ManageHUDAlignment();
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
        //rotational "recovery" ()
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

    public static void SetBlackCoverOpacity(float alpha)
    {

    }

    bool fadingBlackCover;
    public static void FadeBlackCoverOpacity(float targetAlpha)
    {
        if (Mathf.Abs(self.blackCoverSpriteRenderer.color.a - targetAlpha) < .01f) { return; }

        self.fadingBlackCover = true;
        self.blackCoverTargetAlpha = targetAlpha;
    }

    void ProcessFading()
    {
        if (fadingBlackCover)
        {
            if (blackCoverSpriteRenderer.color.a - blackCoverTargetAlpha > .01f) { blackCoverSpriteRenderer.color -= fadeRate; }
            else if (blackCoverSpriteRenderer.color.a - blackCoverTargetAlpha < .01f) { blackCoverSpriteRenderer.color -= fadeRate; }
            else
            {
                blackCoverSpriteRenderer.color = new Color(0,0,0,blackCoverTargetAlpha);
                fadingBlackCover = false;
            }
        }

        if (fadingVignette)
        {
            if (vignetteRenderer.color.a > 0)
            {
                vignetteRenderer.color -= fadeRate;
            }
            else
            {
                color = vignetteRenderer.color;
                color.a = 0;
                vignetteRenderer.color = color;
                fadingVignette = false;
            }
        }
    }

    bool fadingVignette;
    public static void SetVignetteOpacity(float alpha)
    {
        self.fadingVignette = true;

        self.color = self.vignetteRenderer.color;
        self.color.a = alpha;
        self.vignetteRenderer.color = self.color;
    }

    float screenXSize, screenYSize, lastScreenXSize, lastScreenYSize;
    bool CalculateScreenSize()
    {
        screenYSize = 2 * Camera.main.orthographicSize;
        screenXSize = screenYSize * Camera.main.aspect;

        if (Mathf.Abs(screenXSize-lastScreenXSize) > .001f || Mathf.Abs(screenYSize - lastScreenYSize) > .001f)
        {
            lastScreenXSize = screenXSize;
            lastScreenYSize = screenYSize;
            return true;
        }
        return false;
    }

    void AlignHUDElements()
    {
        vignetteRenderer.transform.localScale = new Vector3(.0894f * screenXSize, .185f * screenYSize, 1);

        Vector3 hudPosition = swordHUDTrfm.localPosition;
        hudPosition.x = screenXSize * -.419f + 4;
        swordHUDTrfm.localPosition = hudPosition;
    }

    void ManageHUDAlignment()
    {
        if (alignHUDTimer > 0) { alignHUDTimer--; }
        else
        {
            alignHUDTimer = 100;
            if (CalculateScreenSize())
            {
                AlignHUDElements();
            }
        }
    }
}
