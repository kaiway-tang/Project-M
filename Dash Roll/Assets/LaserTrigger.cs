using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : MonoBehaviour
{
    [SerializeField] bool state;
    [SerializeField] CrystalLaserEnemy[] laserEnemies;

    private void OnTriggerEnter2D(Collider2D col)
    {
        for (int i = 0; i < 4; i++)
        {
            laserEnemies[i].active = state;
        }
    }
}
