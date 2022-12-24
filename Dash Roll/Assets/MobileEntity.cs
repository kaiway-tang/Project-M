using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileEntity : HPEntity
{
    [SerializeField] protected Transform trfm;
    [SerializeField] protected Rigidbody2D rb;
    public bool[] touchingTerrain; //0: touching floor, 1: touching left wall, 2: touching right wall
    protected bool facing;
    protected const bool FACING_LEFT = false, FACING_RIGHT = true;

    int gravityDisable; float defaultGravity;

    Vector2 vect2;
    
    protected void AddXVelocity(float amount, float max)
    {
        vect2 = rb.velocity;

        if (amount > 0)
        {
            if (vect2.x > max)
            {
                return;
            }
            else
            {
                vect2.x += amount;
                if (vect2.x > max)
                {
                    vect2.x = max;
                }
            }
        }
        else
        {
            if (vect2.x < max)
            {
                return;
            }
            else
            {
                vect2.x += amount;
                if (vect2.x < max)
                {
                    vect2.x = max;
                }
            }
        }

        rb.velocity = vect2;
    }
    protected void SetXVelocity(float value)
    {
        vect2 = rb.velocity;
        vect2.x = value;
        rb.velocity = vect2;
    }

    protected void AddYVelocity(float amount, float max)
    {
        vect2 = rb.velocity;
        vect2.y += amount;

        vect2 = rb.velocity;

        if (amount > 0)
        {
            if (vect2.y > max)
            {
                return;
            }
            else
            {
                vect2.y += amount;
                if (vect2.y > max)
                {
                    vect2.y = max;
                }
            }
        }
        else
        {
            if (vect2.y < max)
            {
                return;
            }
            else
            {
                vect2.y += amount;
                if (vect2.y < max)
                {
                    vect2.y = max;
                }
            }
        }

        rb.velocity = vect2;
    }
    protected void SetYVelocity(float value)
    {
        vect2 = rb.velocity;
        vect2.y = value;
        rb.velocity = vect2;
    }

    protected void ApplyXFriction(float amount)
    {
        vect2 = rb.velocity;

        if (vect2.x > 0)
        {
            vect2.x -= amount;
            if (vect2.x < 0)
            {
                vect2.x = 0;
            }
        } else
        {
            vect2.x += amount;
            if (vect2.x > 0)
            {
                vect2.x = 0;
            }
        }

        rb.velocity = vect2;
    }

    protected void ApplyDirectionalFriction(float amount)
    {
        //TODO
    }

    protected void SetVelocity(float x, float y)
    {
        vect2.x = x; vect2.y = y;
        rb.velocity = vect2;
    }

    public bool IsFacingRight()
    {
        return facing;
    }
    public bool IsFacingLeft()
    {
        return !facing;
    }

    protected bool IsTouchingGround()
    {
        return touchingTerrain[0];
    }
    protected bool IsTouchingLeftWall()
    {
        return touchingTerrain[1];
    }
    protected bool IsTouchingRightWall()
    {
        return touchingTerrain[2];
    }

    protected void DisableGravity()
    {
        if (gravityDisable < 1)
        {
            defaultGravity = rb.gravityScale;
            rb.gravityScale = 0;
        }
        gravityDisable++;
    }
    protected void EnableGravity()
    {
        gravityDisable--;
        if (gravityDisable < 1)
        {
            rb.gravityScale = defaultGravity;
        }
    }
}
