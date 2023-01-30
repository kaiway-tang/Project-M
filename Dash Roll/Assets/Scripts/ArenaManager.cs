using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;
    [SerializeField] int[] spawnTimes;
    int timer, currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer == spawnTimes[currentIndex])
        {
            
        }

        timer++;
    }
}
