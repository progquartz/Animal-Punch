using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool IsEnemyHasHealth = false;
    public ActorsStat stat;

    public InteractableActor actorPhysics;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actorPhysics = GetComponent<InteractableActor>();
        actorPhysics.Init(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
