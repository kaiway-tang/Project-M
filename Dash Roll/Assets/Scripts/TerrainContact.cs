using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainContact : MonoBehaviour
{
    [SerializeField] MobileEntity mobileEntity;
    enum ContactType { ground, front, ceiling, backLow, backHigh, }
    int contacts;
    [SerializeField] ContactType contactType;
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            contacts++;
            mobileEntity.touchingTerrain[(int)contactType] = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            contacts--;
            if (contacts < 1)
            {
                mobileEntity.touchingTerrain[(int)contactType] = false;
            }
        }
    }
}
