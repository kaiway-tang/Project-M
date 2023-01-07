using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbox : MonoBehaviour
{
    [SerializeField] static Transform trfm;
    static Vector2 vect2;
    // Start is called before the first frame update
    void Awake()
    {
        trfm = transform;
    }

    public static Quaternion GetQuaternionToPlayerHead(Vector3 position)
    {
        trfm.up = PlayerMovement.headTrfm.position - position;
        return trfm.rotation;
    }
    public static Quaternion GetQuaternionToPlayerPredicted(Vector2 position, int ticks)
    {
        trfm.up = PlayerMovement.PredictedPosition(ticks) - position;
        return trfm.rotation;
    }

    public static Transform GetTransformToPlayer(Vector3 position)
    {
        trfm.up = PlayerMovement.trfm.position - position;
        return trfm;
    }

    public static Vector3 GetUnitVectorToPlayer(Vector3 position)
    {
        trfm.up = PlayerMovement.trfm.position - position;
        return trfm.up;
    }

    public static bool InBoxDistance(Vector2 position1, Vector2 position2, float distance)
    {
        return Mathf.Abs(position1.x - position2.x) < distance && Mathf.Abs(position1.y - position2.y) < distance;
    }
}
