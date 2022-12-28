using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int animatorState, firstEmptyQueSlot, activeAnimations, defaultAnimation;
    [SerializeField] protected int[] animationQueID, animationQueDuration, animationPriority;

    protected void FixedUpdate()
    {
        for (int i = 0; i < animationQueDuration.Length; i++)
        {
            if (animationQueDuration[i] > 0)
            {
                animationQueDuration[i]--;

                if (animationQueDuration[i] < 1)
                {
                    animationQueID[i] = -1;
                    activeAnimations--;

                    animatorState = -1;

                    if (activeAnimations > 0)
                    {
                        RequestAnimatorState(animationQueID[IndexOfGreatestValue(animationQueID)]);
                    }
                    else
                    {
                        RequestAnimatorState(defaultAnimation);
                    }
                }
            }
        }
    }

    public bool RequestAnimatorState(int state) //returns true if requested animation is set
    {
        if (animatorState != state)
        {
            if (animatorState < 0 || animationPriority[state] >= animationPriority[animatorState])
            {
                animator.SetInteger("State", state);
                animatorState = state;
                return true;
            }
            else
            {
                defaultAnimation = state;
            }
        }
        return false;
    }

    public void QueAnimation(int animationID, int duration)
    {
        if (activeAnimations > 0)
        {
            firstEmptyQueSlot = -1;

            for (int i = 0; i < activeAnimations; i++)
            {
                if (firstEmptyQueSlot == -1 && animationQueID[i] == -1) //no empty que slot found && found empty que slot
                {
                    firstEmptyQueSlot = i;
                }
                if (animationQueID[i] == animationID)
                {
                    if (animationQueDuration[i] < duration)
                    {
                        animationQueDuration[i] = duration;
                    }
                    return;
                }
            }

            if (firstEmptyQueSlot == -1)
            {
                Debug.Log("OH NO ARRAY TOOOO SMALLLL");
                return;
            }
        }
        else
        {
            firstEmptyQueSlot = 0;
        }

        animationQueID[firstEmptyQueSlot] = animationID;
        animationQueDuration[firstEmptyQueSlot] = duration;
        RequestAnimatorState(animationID);

        activeAnimations++;
    }

    int indexGreatest;
    int IndexOfGreatestValue(int[] input)
    {
        indexGreatest = 0;
        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] > input[indexGreatest])
            {
                indexGreatest = i;
            }
        }

        return indexGreatest;
    }
}
