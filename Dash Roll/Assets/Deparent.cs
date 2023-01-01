using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deparent : MonoBehaviour
{
    [SerializeField] Deparent deparent;
    [SerializeField] Transform[] trfms;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < trfms.Length; i++)
        {
            trfms[i].parent = null;
        }
        Destroy(deparent);
    }
}
