using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManagerIII : MonoBehaviour
{
    public static ElevatorManagerIII self;
    public static Transform trfm;
    [SerializeField] bool rising;
    public Vector3 riseRate;

    [SerializeField] Rigidbody2D[] enemyRBs;
    [SerializeField] float[] yThresholds;
    [SerializeField] Vector2[] velocities;
    int currentTriggerIndex;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<ElevatorManagerIII>();
        trfm = transform;

        CameraController.self.SetMode(CameraController.ELEVATOR);

        for (int i = 0; i < enemyRBs.Length; i++)
        {
            yThresholds[i] = enemyRBs[i].transform.position.y - 14.5f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.position += riseRate;

        while (currentTriggerIndex < enemyRBs.Length && trfm.position.y >= yThresholds[currentTriggerIndex])
        {
            if (trfm.position.y < yThresholds[currentTriggerIndex] + 2) { enemyRBs[currentTriggerIndex].velocity = velocities[currentTriggerIndex]; }
            currentTriggerIndex++;
        }
    }
}
