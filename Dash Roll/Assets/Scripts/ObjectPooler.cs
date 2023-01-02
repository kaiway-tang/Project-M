using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] PooledObject[] pooledObjects;
    GameObject[] gameObjects;
    bool[] objectReady;

    static Vector3 instantiationRotation;

    private void Start()
    {
        objectReady = new bool[pooledObjects.Length];
        gameObjects = new GameObject[pooledObjects.Length];
        for (int i = 0; i < pooledObjects.Length; i++)
        {
            pooledObjects[i].Setup(this, i + 1);
            gameObjects[i] = pooledObjects[i].gameObject;
            objectReady[i] = true;
        }
    }

    public void Instantiate(Vector2 position, float zRotation = 0)
    {
        instantiationRotation.z = zRotation;

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (objectReady[i])
            {
                gameObjects[i].SetActive(true);
                pooledObjects[i].trfm.position = position;
                pooledObjects[i].trfm.localEulerAngles = instantiationRotation;
                objectReady[i] = false;

                return;
            }
        }

        Instantiate(prefab, position, Quaternion.Euler(instantiationRotation));
        Debug.Log("not enough "+ prefab);
    }

    public void SetReady(int pObjectID)
    {
        objectReady[pObjectID] = true;
    }
}
