using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject[] objects;
    [SerializeField] Transform[] trfms;
    public bool[] objectReady;

    Vector3 instantiationRotation;
    
    public void Instantiate(Vector2 position, float zRotation = 0)
    {
        instantiationRotation.z = zRotation;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objectReady[i])
            {
                objects[i].SetActive(true);
                trfms[i].position = position;
                trfms[i].localEulerAngles = instantiationRotation;
                objectReady[i] = false;

                return;
            }
        }

        Instantiate(prefab, position, Quaternion.Euler(instantiationRotation));
        Debug.Log("not enough "+ prefab);
    }
}
