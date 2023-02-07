using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buzzsaw : MonoBehaviour
{
    [SerializeField] int damage, rotationSpeed;
    int takeDamageResult;

    [SerializeField] Vector3[] destinations;
    [SerializeField] Transform trfm;
    int destinationIndex;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        //if (Mathf.Abs(trfm.position.x - destinations[destinationIndex].x) < .1f && Mathf.Abs(trfm.position.y - destinations[destinationIndex].y) < .1f) { }
        //SelectNextPosition();

        trfm.Rotate(Vector3.forward * rotationSpeed);
    }

    void SelectNextPosition()
    {
        destinationIndex++;
        trfm.up = destinations[destinationIndex] - trfm.position;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14)
        {
            takeDamageResult = col.GetComponent<HPEntity>().TakeDamage(damage, HPEntity.EntityTypes.Neutral);
            return;
        }
        takeDamageResult = HPEntity.IGNORED;
    }
}
