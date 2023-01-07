using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimator : AnimationController
{
    public ReferenceState
        Idle = new ReferenceState(0, 1),
        Walk = new ReferenceState(1, 1),
        Attack = new ReferenceState(2, 10),
        Leap = new ReferenceState(3, 1),
        Die = new ReferenceState(4, 25);

    // Start is called before the first frame update
    new void Start()
    {
        currentState = new ActiveState(Idle);
        defaultState = new ActiveState(Idle);

        animationQue = new ActiveState[] { new ActiveState(), new ActiveState(), new ActiveState() };
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
