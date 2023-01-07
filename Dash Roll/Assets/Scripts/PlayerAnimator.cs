using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : AnimationController
{
    //public const int IDLE = 0, RUN = 1, JUMP = 2, FALL = 3, ATTACK_1 = 4, ATTACK_2 = 5, CLING_BACK = 6, CLING_FRONT = 7, ROLL = 8;

    public ReferenceState
        Idle = new ReferenceState(0, 1),
        Run = new ReferenceState(1, 1),
        Jump = new ReferenceState(2, 1),
        Fall = new ReferenceState(3, 1),
        LightAttack1 = new ReferenceState(9, 100),
        LightAttack2 = new ReferenceState(4, 110),
        HeavyAttack = new ReferenceState(5, 150),
        ClingBack = new ReferenceState(6, 1),
        ClingFront = new ReferenceState(7, 1),
        Roll = new ReferenceState(8, 50),
        Kick = new ReferenceState(10, 60),
        CastHorizontal = new ReferenceState(11, 60);

    // Start is called before the first frame update
    new void Start()
    {
        currentState = new ActiveState(Idle);
        defaultState = new ActiveState(Idle);

        animationQue = new ActiveState[] { new ActiveState(), new ActiveState(), new ActiveState(), new ActiveState() };
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
