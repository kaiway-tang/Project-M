using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAttack : Attack
{
    public const int RIGHT = 0, LEFT = 1, DOWN = 2, UP = 3;
    protected int direction;
    [SerializeField] protected Vector2[] knockbackDirections;
    // Start is called before the first frame update

    public void Activate(int pDirection) //attacks with > 2 directions
    {
        direction = pDirection;
        Activate();
    }

    public void Activate(bool rightwards) //attacks with 2 directions (left or right
    {
        if (rightwards) { direction = RIGHT; }
        else { direction = LEFT; }
        Activate();
    }

    protected new void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer > 10 && col.gameObject.layer < 14) 
        {
            takeDamageResult = col.GetComponent<HPEntity>().TakeDamage(damage, entityType, knockbackDirections[direction]);
            return;
        }
        takeDamageResult = HPEntity.IGNORED;
    }
}
