using UnityEngine;

public enum ActorBehaviourType
{
    CowardBehaviour,
    AggressiveBehaviour,
}
public abstract class ActorBehaviour
{
    public abstract void Act();
}
