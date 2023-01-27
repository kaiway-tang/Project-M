using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchPuzzle : MonoBehaviour
{
    [SerializeField] bool[] torchLit;
    [SerializeField] SpriteRenderer[] spriteRenderers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) { ShiftLeft(); }
        if (Input.GetKeyDown(KeyCode.RightShift)) { ToggleAll(); }
        if (Input.GetKeyDown(KeyCode.Equals)) { Add(); }
    }

    void Add()
    {
        for (int i = 7; i >= 0; i--)
        {
            if (!torchLit[i])
            {
                torchLit[i] = true;

                for (int k = i + 1; k < 8; k++)
                {
                    torchLit[k] = false;
                }

                break;
            }
        }

        UpdateTorches();
    }

    void ShiftLeft()
    {
        for (int i = 0; i < 7; i++)
        {
            torchLit[i] = torchLit[i + 1];
        }
        torchLit[7] = false;

        UpdateTorches();
    }
    void ToggleAll()
    {
        for (int i = 0; i < 8; i++)
        {
            torchLit[i] = !torchLit[i];
        }

        UpdateTorches();
    }

    void UpdateTorches()
    {
        for (int i = 0; i < 8; i++)
        {
            if (torchLit[i]) { spriteRenderers[i].color = Color.white; }
            else { spriteRenderers[i].color = Color.black; }
        }
    }
}
