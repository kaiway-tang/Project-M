using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerEnemy : Enemy
{
    [SerializeField] GameObject fireball, firebolts, meteor, skull;
    [SerializeField] int attackTimer, attack;
    
    new void Start()
    {
        base.Start();
    }


    bool everyTwo;
    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (attackTimer > 0)
        {
            attackTimer--;
        }
        else
        {
            //attack = Random.Range(0,4);
            attack = 1;

            if (attack == 0) //explosive fireball
            {
                Instantiate(fireball, trfm.position, Toolbox.GetQuaternionToPlayer(trfm.position));
            }
            else if (attack == 1) //triple bolt
            {
                Debug.Log("creating firebolts")
                Instantiate(firebolts, trfm.position, Toolbox.GetQuaternionToPlayer(trfm.position));
            } else if (attack == 2) //meteor
            {
                Instantiate(meteor);
            } else if (attack == 3) //summon skull
            {
                Instantiate(skull, trfm.position, trfm.rotation);
            }

            attackTimer = Random.Range(150, 50);
        }


        everyTwo = !everyTwo;
        if (everyTwo) { EveryTwo(); }
    }
    void EveryTwo()
    {
        FacePlayer();
    }
}
