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
    public abstract void CheckCondition();
    public abstract void BehaveOnUpdate();

    public abstract void Init(Enemy owner);

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
