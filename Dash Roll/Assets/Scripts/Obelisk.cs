using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obelisk : MonoBehaviour
{
    [SerializeField] bool invisible;
    [SerializeField] Animator animator;
    [SerializeField] Transform trfm;
    [SerializeField] Vector3 bob;
    public static Vector3 lastObeliskPosition;
    public static int lastObeliskScene;
    int animationTimer, animationIdleDelay;
    bool claimed;

    private void Start()
    {
        if (invisible)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (!invisible)
        {
            if (animationTimer > 0)
            {
                trfm.position += bob;
                if (animationTimer > 100) { animationTimer = -101; }
            }
            else
            {
                trfm.position -= bob;
            }

            if (animationIdleDelay > 0)
            {
                if (animationIdleDelay == 1)
                {
                    animator.SetInteger("state", 1);
                }
                animationIdleDelay--;
            }

            animationTimer++;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 1)
        {
            if (!claimed)
            {
                lastObeliskPosition = trfm.position;
                lastObeliskScene = GameManager.GetCurrentScene();

                claimed = true;
                animator.SetInteger("state", 1);

                if (!invisible)
                {
                    Player.PlayerHeal(999);
                    Player.AddMana(999);
                }
            }
            /*
            else
            {
                animator.SetInteger("state", 2);
                animationIdleDelay = 10;
            }
            */
        }
    }
}
