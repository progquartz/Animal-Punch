using UnityEngine;

public enum ActorBehaviourType
{
    CowardBehaviour,
    AggressiveBehaviour,
    WanderingBehaviour,
    SmartBehaviour,
}
public abstract class ActorBehaviour
{
    protected EnemyMoving owner;
    protected Transform playerTransform;
    public abstract void CheckCondition();
    public abstract void BehaveOnUpdate();

    public abstract void InitStateList();

    public virtual void Init(EnemyMoving owner)
    {
        if (GameManager.Instance.IsTimeStop) return;
        this.owner = owner;
        playerTransform = Player.Instance.PlayerTransform;
    }

    public static ActorBehaviour GetActorBehaviour(ActorBehaviourType type)
    {
        switch (type)
        {
            case ActorBehaviourType.CowardBehaviour:
                return new CowardBehaviour();
            case ActorBehaviourType.AggressiveBehaviour:
                return new AggressiveBehaviour();
            case ActorBehaviourType.WanderingBehaviour:
                return new WanderingBehaviour();
            case ActorBehaviourType.SmartBehaviour:
                return new SmartBehaviour();
        }
        return null;
    }
}
