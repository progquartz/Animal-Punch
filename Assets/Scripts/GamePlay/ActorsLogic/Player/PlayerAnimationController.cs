using UnityEngine;

public class PlayerAnimationController : AnimalAnimationController
{
    private Player owner;

    public void Init(Player owner)
    {
        this.owner = owner;
        base.Init();
    }
    public void ChangeAnimationDependOnSpeed()
    {
        float speed = owner.Stat.RigidbodySpeed;
        if (speed > float.Epsilon)
        {
            float ratio = owner.Stat.RigidbodySpeed / owner.Stat.StandardAnimationSpeed;
            ChangeAnimation(AnimalAnimation.Run);
        }
        else
        {
            ChangeAnimation(AnimalAnimation.Idle_A);
        }
    }
}
