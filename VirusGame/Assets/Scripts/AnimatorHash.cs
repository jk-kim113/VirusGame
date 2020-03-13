using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorHash
{
    public static readonly int WALK = Animator.StringToHash("IsWalk");
    public static readonly int DIE = Animator.StringToHash("IsDie");
    public static readonly int EAT = Animator.StringToHash("IsEat");
    public static readonly int RUN = Animator.StringToHash("IsRun");
    public static readonly int ATTACK = Animator.StringToHash("IsAttack");
}
