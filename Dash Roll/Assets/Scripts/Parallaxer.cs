using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    [SerializeField] Transform cameraTrfm;
    [SerializeField] Vector3 referencePoint;
    [SerializeField] Transform[] backgrounds;
    [SerializeField] float[] rates;

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
            backgrounds.
        }
    }
}
