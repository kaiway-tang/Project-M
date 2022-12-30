using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public struct State
    {
        public int ID, priority;

        public State(int pID, int pPriority)
        {
            ID = pID;
            priority = pPriority;
        }
    }

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

    public bool RequestAnimatorState(State state) //returns true if requested animation is set
    {
        if (animatorState != state.ID)
        {
            if (animatorState < 0 || animationPriority[state.ID] >= animationPriority[animatorState])
            {
                animator.SetInteger("State", state.ID);
                animatorState = state.ID;
                return true;
            }
            else
            {
                defaultAnimation = state.ID;
            }
        }
        return false;
    }

    public void QueAnimation(State state, int duration)
    {
        if (activeAnimations > 0)
        {
            firstEmptyQueSlot = -1;

            for (int i = 0; i < animationQueID.Length; i++)
            {
                if (firstEmptyQueSlot == -1 && animationQueID[i] == -1) //no empty que slot found && found empty que slot
                {
                    firstEmptyQueSlot = i;
                }
                if (animationQueID[i] == state.ID)
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

        animationQueID[firstEmptyQueSlot] = state.ID;
        animationQueDuration[firstEmptyQueSlot] = duration;
        RequestAnimatorState(state);

        activeAnimations++;
    }

    public void DeQueAnimation(int animationID)
    {
        for (int i = 0; i < animationQueID.Length; i++)
        {
            if (animationQueID[i] == animationID)
            {
                animationQueID[i] = -1;
                animationQueDuration[i] = 0;
                activeAnimations--;
                return;
            }
        }
    }

    int indexGreatest;
    int IndexOfGreatestValue(State[] input)
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
