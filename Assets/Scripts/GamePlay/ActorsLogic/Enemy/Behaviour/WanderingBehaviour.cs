using UnityEngine;

public class WanderingBehaviour : ActorBehaviour
{
    private Enemy owner;

    // Coward Behaviour의 기본 상태는 Wander
    IActorPattern CurrentPattern;
    WanderPattern WanderPattern;

    public override void CheckCondition()
    {
        ChangePattern(WanderPattern);
    }

    public override void BehaveOnUpdate()
    {
        CurrentPattern.ActPattern();
    }

    public override void Init(Enemy owner)
    {
        this.owner = owner;
        WanderPattern = new WanderPattern();
        WanderPattern.Init(owner);
        CurrentPattern = WanderPattern;
    }



    private void ChangePattern(IActorPattern target)
    {
        // 만약 다른 패턴일 경우.
        if (CurrentPattern.GetType() != target.GetType())
        {
            CurrentPattern.ExitPattern();
            CurrentPattern = target;
            CurrentPattern.EnterPattern();
        }
    }

}
