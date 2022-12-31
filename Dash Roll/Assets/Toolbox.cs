using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbox : MonoBehaviour
{
    [SerializeField] static Transform trfm;
    // Start is called before the first frame update
    void Awake()
    {
        trfm = transform;
    }

    public static Quaternion GetQuaternionToPlayer(Vector3 position)
    {
        trfm.up = PlayerMovement.trfm.position - position;
        return trfm.rotation;
    }
}
