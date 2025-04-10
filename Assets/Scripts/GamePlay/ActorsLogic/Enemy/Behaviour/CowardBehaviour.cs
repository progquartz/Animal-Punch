using NUnit.Framework;
using UnityEngine;

public class CowardBehaviour : ActorBehaviour
{
    private EnemyMoving owner;
    private float detectionRange = 15f;

    // Coward Behaviour의 기본 상태는 Wander
    IActorPattern CurrentPattern;
    WanderPattern WanderPattern;
    FleePattern FleePattern;
    

    
    public override void CheckCondition()
    {
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
        Transform playerPos = Player.Instance.PlayerTransform;

        float dist = Vector3.Distance(playerPos.position , owner.transform.position);
        //Debug.Log($"dist = {dist}");
        if(dist < detectionRange)
        {
            return true;
        }
        return false;
    }

    public override void Init(EnemyMoving owner)
    {
        this.owner = owner;
        FleePattern = new FleePattern();
        WanderPattern = new WanderPattern();
        FleePattern.Init(owner);
        WanderPattern.Init(owner);
        CurrentPattern = WanderPattern;
    }


}
