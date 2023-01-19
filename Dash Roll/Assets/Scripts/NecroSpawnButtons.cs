using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroSpawnButtons : HPEntity
{
    static int buttons;
    [SerializeField] GameObject necromancer;
    // Start is called before the first frame update
    private new void FixedUpdate()
    {
        if (HP <= 0)
        {
            buttons++;
            if (buttons == 7)
            {
                necromancer.transform.position = trfm.position + new Vector3(0,10,0);
                necromancer.SetActive(true);
            }
            Destroy(gameObject);
        }
        base.FixedUpdate();
    }
}
