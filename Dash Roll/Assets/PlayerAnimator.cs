using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : AnimationController
{
    public const int IDLE = 0, RUN = 1, JUMP = 2, FALL = 3, ATTACK_1 = 4, ATTACK_2 = 5, CLING_BACK = 6, CLING_FRONT = 7;

    // Start is called before the first frame update
    void Start()
    {
        animationQueID = new int[3] { -1, -1, -1 };
        animationQueDuration = new int[3];
        animationPriority = new int[8] {1, 1, 1, 1, 10, 11, 1, 1};
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
