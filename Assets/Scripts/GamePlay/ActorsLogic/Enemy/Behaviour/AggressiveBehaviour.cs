using UnityEngine;

public class AggressiveBehaviour : ActorBehaviour
{
    private EnemyMoving owner;
    private float detectionRange = 15f;

    // Coward Behaviour�� �⺻ ���´� Wander
    IActorPattern CurrentPattern;
    WanderPattern WanderPattern;
    AttackPattern AttackPattern;

    public override void CheckCondition()
    {
        if (IsPlayerInDetectionRange())
        {
            ChangePattern(AttackPattern);
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

    public override void Init(EnemyMoving owner)
    {
        this.owner = owner;
        AttackPattern = new AttackPattern();
        WanderPattern = new WanderPattern();
        AttackPattern.Init(owner);
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

    private bool IsPlayerInDetectionRange()
    {
        Transform playerPos = Player.Instance.PlayerTransform;

        float dist = Vector3.Distance(playerPos.position, owner.transform.position);
        //Debug.Log($"dist = {dist}");
        if (dist < detectionRange)
        {
            return true;
        }
        return false;
    }

}
