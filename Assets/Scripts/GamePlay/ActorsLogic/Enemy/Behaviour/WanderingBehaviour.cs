using UnityEngine;

public class WanderingBehaviour : ActorBehaviour
{
    private EnemyMoving owner;

    // Coward Behaviour�� �⺻ ���´� Wander
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

    public override void Init(EnemyMoving owner)
    {
        this.owner = owner;
        WanderPattern = new WanderPattern();
        WanderPattern.Init(owner);
        CurrentPattern = WanderPattern;
    }



    private void ChangePattern(IActorPattern target)
    {
        // ���� �ٸ� ������ ���.
        if (CurrentPattern.GetType() != target.GetType())
        {
            CurrentPattern.ExitPattern();
            CurrentPattern = target;
            CurrentPattern.EnterPattern();
        }
    }

}
