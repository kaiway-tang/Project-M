using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPGemManager : MonoBehaviour
{
    [SerializeField] Transform[] fillTrfm;
    [SerializeField] GameObject[] gemObj;
    static HPGemManager self;

    static Vector3 vect3;
    
    void Start()
    {
        self = GetComponent<HPGemManager>();

        for (int i = 0; i < Player.soulShards; i++)
        {
            SetGemActive(i, true);
        }
        SetScalerPercent(Player.soulShards, Player.currentShardPercent);
    }

    public static void SetScalerPercent(int index, int percent) //0 - 100
    {
        if (index >= self.gemObj.Length) { return; }

        vect3.x = 0; vect3.z = 0;
        vect3.y = -2.68f * (1 - percent / 120f);

        self.fillTrfm[index].localPosition = vect3;
    }

    public static void SetGemActive(int index, bool active)
    {
        if (index >= self.gemObj.Length) { return; }

        self.gemObj[index].SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
