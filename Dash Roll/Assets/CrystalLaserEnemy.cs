using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalLaserEnemy : Enemy
{
    [SerializeField] int timer;
    [SerializeField] Laser laserScript;
    public bool active;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        timer = Random.Range(50,200);
    }
    
    new void FixedUpdate()
    {
        if (active)
        {
            base.FixedUpdate();

            if (timer > 0)
            {
                timer--;
            }
            else
            {
                laserScript.Fire();
                timer = Random.Range(100, 250);
            }
        }
    }
}
