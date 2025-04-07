using UnityEngine;

public class SmartBehaviour : ActorBehaviour
{
    private Enemy owner;
    private float detectionRange = 8f;

    // Coward Behaviour의 기본 상태는 Wander
    IActorPattern CurrentPattern;
    FleePattern FleePattern;
    AttackPattern AttackPattern;

    public override void CheckCondition()
    {
        if (IsEnemyHealthGood())
        {
            ChangePattern(AttackPattern);
        }
        else
        {
            ChangePattern(FleePattern);
        }
    }

    public override void BehaveOnUpdate()
    {
        CurrentPattern.ActPattern();
    }

    public override void Init(Enemy owner)
    {
        this.owner = owner;
        FleePattern = new FleePattern();
        AttackPattern = new AttackPattern();
        FleePattern.Init(owner);
        AttackPattern.Init(owner);
        CurrentPattern = AttackPattern;
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

    private bool IsEnemyHealthGood()
    {
        if (owner.stat.HP >= owner.stat.MaxHP / 2)
        {
            return true;
        }
        return false;
    }

}
