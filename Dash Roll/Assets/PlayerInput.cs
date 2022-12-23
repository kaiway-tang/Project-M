using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    static KeyCode UpKey, UpKey1, DownKey, DownKey1, LeftKey, LeftKey1, RightKey, RightKey1;
    static KeyCode AttackKey, DashRollKey;

    static bool firstLoad;

    private void Start()
    {
        if (firstLoad) { return; }

        UpKey = KeyCode.W;
        UpKey1 = KeyCode.UpArrow;

        DownKey = KeyCode.S;
        DownKey1 = KeyCode.DownArrow;

        LeftKey = KeyCode.A;
        LeftKey1 = KeyCode.LeftArrow;

        RightKey = KeyCode.D;
        RightKey1 = KeyCode.RightArrow;


        AttackKey = KeyCode.U;

        DashRollKey = KeyCode.I;


        firstLoad = true;
    }

    public static bool UpPressed()
    {
        return Input.GetKeyDown(UpKey) || Input.GetKeyDown(UpKey1);
    }
    public static bool UpHeld()
    {
        return Input.GetKey(UpKey) || Input.GetKey(UpKey1);
    }
    public static bool UpReleased()
    {
        return Input.GetKeyUp(UpKey) || Input.GetKeyUp(UpKey1);
    }

    public static bool DownPressed()
    {
        return Input.GetKeyDown(DownKey) || Input.GetKeyDown(DownKey1);
    }
    public static bool DownHeld()
    {
        return Input.GetKey(DownKey) || Input.GetKey(DownKey1);
    }
    public static bool DownReleased()
    {
        return Input.GetKeyUp(DownKey) || Input.GetKeyUp(DownKey1);
    }

    public static bool LeftPressed()
    {
        return Input.GetKeyDown(LeftKey) || Input.GetKeyDown(LeftKey1);
    }
    public static bool LeftHeld()
    {
        return Input.GetKey(LeftKey) || Input.GetKey(LeftKey1);
    }
    public static bool LeftReleased()
    {
        return Input.GetKeyUp(LeftKey) || Input.GetKeyUp(LeftKey1);
    }

    public static bool RightPressed()
    {
        return Input.GetKeyUp(RightKey) || Input.GetKeyUp(RightKey1);
    }
    public static bool RightHeld()
    {
        return Input.GetKey(RightKey) || Input.GetKey(RightKey1);
    }
    public static bool RightReleased()
    {
        return Input.GetKeyDown(RightKey) || Input.GetKeyDown(RightKey1);
    }

    public static bool AttackPressed()
    {
        return Input.GetKeyDown(AttackKey);
    }
    public static bool AttackHeld()
    {
        return Input.GetKey(AttackKey);
    }
    public static bool AttackReleased()
    {
        return Input.GetKeyUp(AttackKey);
    }

    public static bool DashRollPressed()
    {
        return Input.GetKeyDown(DashRollKey);
    }
}
