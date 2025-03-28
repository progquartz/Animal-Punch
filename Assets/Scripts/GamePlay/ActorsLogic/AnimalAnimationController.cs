using System.Collections.Generic;
using UnityEngine;

public enum AnimalAnimation
{
    Attack = 0,
    Bounce = 1,
    Clicked = 2,
    Death = 3,
    Eat = 4,
    Fear = 5,
    Fly = 6,
    Hit = 7,
    Walk = 8,
    Idle_A = 9, Idle_B = 10, Idle_C = 11,
    Jump = 12,
    Roll = 13,
    Run = 14,
    Sit = 15,
    Swim = 16
}

public enum AnimalShapeKey
{
    Eyes_Annoyed = 0,
    Eyes_Blink = 1,
    Eyes_Cry = 2,
    Eyes_Dead = 3,
    Eyes_Excited = 4,
    Eyes_Happy = 5,
    Eyes_LookDown = 6,
    Eyes_LookIn = 7,
    Eyes_LookOut = 8,
    Eyes_LookUp = 9,
    Eyes_Rabid = 10,
    Eyes_Sad = 11,
    Eyes_Shrink = 12,
    Eyes_Sleep = 13 ,
    Eyes_Spin = 14,
    Eyes_Squint = 15,
    Eyes_Trauma = 16,
    Sweat_L = 17,
    Sweat_R = 18,
    Teardrop_L = 19,
    Teardrop_R = 20
}

public class AnimalAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public string currentAnimation;

    private List<string> animationList = new List<string>
                                            {   "Attack",
                                                "Bounce",
                                                "Clicked",
                                                "Death",
                                                "Eat",
                                                "Fear",
                                                "Fly",
                                                "Hit",
                                                "Walk",
                                                "Idle_A", "Idle_B", "Idle_C",
                                                "Jump",
                                                "Roll",
                                                "Run",
                                                "Sit",
                                                "Swim"
                                            };
    private List<string> shapekeyList = new List<string>
                                            {   "Eyes_Annoyed",
                                                "Eyes_Blink",
                                                "Eyes_Cry",
                                                "Eyes_Dead",
                                                "Eyes_Excited",
                                                "Eyes_Happy",
                                                "Eyes_LookDown",
                                                "Eyes_LookIn",
                                                "Eyes_LookOut",
                                                "Eyes_LookUp",
                                                "Eyes_Rabid",
                                                "Eyes_Sad",
                                                "Eyes_Shrink",
                                                "Eyes_Sleep",
                                                "Eyes_Spin",
                                                "Eyes_Squint",
                                                "Eyes_Trauma",
                                                "Sweat_L",
                                                "Sweat_R",
                                                "Teardrop_L",
                                                "Teardrop_R"
                                            };

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimation(AnimalAnimation animationType)
    {
        if (animator != null)
        {
            // 같은 애니메이션이 호출되지 않았을 경우에만 바뀜.
            if (animationList[(int)animationType] != currentAnimation)
            {
                animator.CrossFade(animationList[(int)animationType], 0.2f);
            }
        }
    }
}
