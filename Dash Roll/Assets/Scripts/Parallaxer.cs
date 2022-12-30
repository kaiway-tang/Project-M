using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    [SerializeField] Transform cameraTrfm;
    [SerializeField] Vector2 referencePoint;
    [SerializeField] Transform[] backgrounds;
    [SerializeField] float rate;
    [SerializeField] Vector2[] ratios;

    Vector2 vect2;

    // Start is called before the first frame update
    void Start()
    {

    }

    void SetReferencePoint(Vector2 position)
    {
        referencePoint = position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            vect2.x = (cameraTrfm.position.x - referencePoint.x) * ratios[i].x * rate;
            vect2.y = (cameraTrfm.position.y - referencePoint.y) * ratios[i].y * rate;

            //vect2.x = referencePoint.x + ratios[i].x * (cameraTrfm.position.x - cameraTrfm.position.x) * rate;
            //vect2.y = referencePoint.y + ratios[i].y * (cameraTrfm.position.y - cameraTrfm.position.y) * rate;\

            backgrounds[i].position = vect2;
        }
    }
}
