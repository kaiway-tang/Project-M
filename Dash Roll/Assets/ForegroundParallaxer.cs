using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundParallaxer : MonoBehaviour
{
    [SerializeField] Transform foregroundLayer;
    [SerializeField] Transform[] foregroundObjects;
    [SerializeField] float rate, yFactor;
    [SerializeField] Transform cameraTrfm;
    [SerializeField] Vector2 referencePoint;

    Vector2 vect2;

    void Start()
    {
        for (int i = 0; i < foregroundObjects.Length; i++)
        {
            vect2.x = foregroundObjects[i].position.x * (1 - rate);
            vect2.y = foregroundObjects[i].position.y * (1 - rate * yFactor);
            foregroundObjects[i].position = vect2;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vect2.x = (cameraTrfm.position.x - referencePoint.x) * rate;
        vect2.y = (cameraTrfm.position.y - referencePoint.y) * rate * yFactor;

        foregroundLayer.position = vect2;
    }
}
