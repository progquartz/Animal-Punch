using UnityEngine;

public class SmartBehaviour : ActorBehaviour
{
    private float limitRange = 25f;

    // Coward Behaviour의 기본 상태는 Wander
    IActorPattern CurrentPattern;
    FleePattern FleePattern;
    AttackPattern AttackPattern;

    public override void Init(EnemyMoving owner)
    {
        base.Init(owner);
        InitStateList();
    }

    public override void InitStateList()
    {
        FleePattern = new FleePattern();
        AttackPattern = new AttackPattern();
        FleePattern.Init(owner);
        AttackPattern.Init(owner);
        CurrentPattern = AttackPattern;
    }


    public override void CheckCondition()
    {
        if(IsPlayerOverLimitRange())
        {
            ChangePattern(AttackPattern);
            return;
        }

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

    private bool IsPlayerOverLimitRange()
    {
        float dist = Vector3.Distance(playerTransform.position, owner.transform.position);
        if (dist > limitRange)
        {
            return true;
        }
        return false;
    }


}
