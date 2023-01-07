using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerAnimator : AnimationController
{
    public ReferenceState
        Idle = new ReferenceState(0, 1),
        Fly = new ReferenceState(1, 1),
        Summon = new ReferenceState(2, 10),
        CastSpell = new ReferenceState(3, 10),
        Die = new ReferenceState(5, 25);

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
