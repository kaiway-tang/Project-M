using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundParallaxer : MonoBehaviour
{
    [SerializeField] Transform foregroundLayer;
    [SerializeField] Transform[] foregroundObjects;
    [SerializeField] float rate;
    [SerializeField] Transform cameraTrfm;
    [SerializeField] Vector2 referencePoint;

    Vector2 vect2;

    void Start()
    {
        for (int i = 0; i < foregroundObjects.Length; i++)
        {
            foregroundObjects[i].position *= 1 - rate;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vect2.x = (cameraTrfm.position.x - referencePoint.x) * rate;
        vect2.y = (cameraTrfm.position.y - referencePoint.y) * rate;

        foregroundLayer.position = vect2;
    }
}
