using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    [SerializeField] Transform cameraTrfm;
    [SerializeField] Vector3 referencePoint;
    [SerializeField] Transform[] backgrounds;
    [SerializeField] float rate;
    [SerializeField] Vector2[] ratios;

    Vector2 vect2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            //vect2.x = initPos[i].x + ratios[i].x * (cameraTrfm.position.x - camInitPos.x) * rate;
            //vect2.y = initPos[i].y + ratios[i].y * (cameraTrfm.position.y - camInitPos.y) * rate;
            backgrounds[i].position = vect2;
        }
    }
}
