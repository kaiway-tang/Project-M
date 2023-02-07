using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    [SerializeField] float loopThreshold, resetDifference;

    [SerializeField] GameObject[] eventObjects;
    [SerializeField] int[] eventTime;

    [SerializeField] Transform[] loopingBackgrounds;
    [SerializeField] Transform[] backgroundTrfms;
    [SerializeField] float[] backgroundDeactivationTime;
    bool[] backgroundActive;
    [SerializeField] Parallaxer parallaxer;
    [SerializeField] Vector3 riseRate;
    public bool backgroundDescending;

    [SerializeField] int eventIndex, backgroundDeactivationIndex, duration;

    Transform trfm;
    bool everyTwo;


    void Start()
    {
        trfm = transform;

        backgroundActive = new bool[backgroundTrfms.Length];
        for (int i = 0; i < backgroundActive.Length; i++)
        {
            backgroundActive[i] = backgroundTrfms[i].gameObject.activeSelf;
        }

        for (int i = 0; i < backgroundDeactivationTime.Length; i++)
        {
            backgroundDeactivationTime[i] *= 50;
        }

        for (int i = 0; i < eventTime.Length; i++)
        {
            eventTime[i] *= 50;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        while (eventIndex < eventTime.Length && duration >= eventTime[eventIndex])
        {
            eventObjects[eventIndex].SetActive(!eventObjects[eventIndex].activeSelf);
            eventIndex++;
        }
        while (backgroundDeactivationIndex < backgroundDeactivationTime.Length && duration >= backgroundDeactivationTime[backgroundDeactivationIndex])
        {
            backgroundActive[backgroundDeactivationIndex] = !backgroundActive[backgroundDeactivationIndex];
            backgroundTrfms[backgroundDeactivationIndex].gameObject.SetActive(backgroundActive[backgroundDeactivationIndex]);
            backgroundDeactivationIndex++;
        }
        duration++;

        if (backgroundDescending)
        {
            for (int i = 0; i < backgroundTrfms.Length; i++)
            {
                if (backgroundActive[i])
                {
                    backgroundTrfms[i].position += riseRate;
                }
            }

            parallaxer.referencePoint.y -= riseRate.y * .02f;
        }

        for (int i = 0; i < loopingBackgrounds.Length; i++)
        {
            if (loopingBackgrounds[i].position.y + loopThreshold < trfm.position.y)
            {
                loopingBackgrounds[i].position += Vector3.up * resetDifference;
            }
        }


        if (everyTwo) { EveryTwo(); }
        everyTwo = !everyTwo;
    }

    void EveryTwo()
    {
        if (Player.trfm.position.y < trfm.position.y) { riseRate.y = .1f; } else
        if (Player.trfm.position.y > trfm.position.y + 16) { riseRate.y = .2f; } else
        {
            riseRate.y = .15f;
        }
    }
}
