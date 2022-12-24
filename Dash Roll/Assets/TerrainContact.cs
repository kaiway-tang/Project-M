using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainContact : MonoBehaviour
{
    [SerializeField] MobileEntity mobileEntity;
    enum ContactType { ground, leftWall, rightWall, ceiling }
    [SerializeField] ContactType contactType;
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            mobileEntity.touchingTerrain[(int)contactType] = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 7)
        {
            mobileEntity.touchingTerrain[(int)contactType] = false;
        }
    }
}
