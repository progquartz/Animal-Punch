using NUnit.Framework;
using UnityEngine;

public class CowardBehaviour : ActorBehaviour
{
    private float detectionRange = 15f;
    private float limitRange = 25f;

    // Coward Behaviour의 기본 상태는 Wander
    IActorPattern CurrentPattern;
    WanderPattern WanderPattern;
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
        WanderPattern = new WanderPattern();
        AttackPattern = new AttackPattern();
        FleePattern.Init(owner);
        WanderPattern.Init(owner);
        AttackPattern.Init(owner);
        CurrentPattern = WanderPattern;
    }


    public override void CheckCondition()
    {
        if(IsPlayerOverLimitRange())
        {
            ChangePattern(AttackPattern);
            return;
        }

        if (IsPlayerInDetectionRange())
        {
            ChangePattern(FleePattern);
        }
        else
        {
            ChangePattern(WanderPattern);
        }
    }

    public override void BehaveOnUpdate()
    {
        if (GameManager.Instance.IsGamePaused) return;
        CurrentPattern.ActPattern();
    }


    private void ChangePattern(IActorPattern target)
    {
        // 만약 다른 패턴일 경우.
        if(CurrentPattern.GetType() != target.GetType())
        {
            CurrentPattern.ExitPattern();
            CurrentPattern = target;
            CurrentPattern.EnterPattern();
        }
    }

    private bool IsPlayerInDetectionRange()
    {

        float dist = Vector3.Distance(playerTransform.position , owner.transform.position);
        if(dist < detectionRange)
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
