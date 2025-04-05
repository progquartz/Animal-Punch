using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyDataSO targetEnemyDataSO;
    public ActorsStat stat;

    public ActorCollision actorPhysics;
    [SerializeField] private Transform enemyTransform;
    private ActorBehaviour actorBehaviour;
    
    // start에서 Init으로 추후에 옮기기.
    public void Init(EnemyDataSO enemyData)
    {
        targetEnemyDataSO = enemyData;
        stat.CopyData(enemyData.ActorsStat);
        if (stat.IsEnemyHasCondition)
        {
            actorBehaviour = ActorBehaviour.GetActorBehaviour(stat.BehaviourType);
            actorBehaviour.Init(this);
        }
        else
        {
            actorBehaviour = null;
        }
        actorPhysics = GetComponent<ActorCollision>();
        actorPhysics.Init(this, enemyTransform);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // factory 제작 및 initiating 이후에 수정해야 함.
        Init(targetEnemyDataSO);
    }

    // Update is called once per frame
    void Update()
    {
        if(stat.IsEnemyHasCondition)
        {
            actorBehaviour.CheckCondition();
            actorBehaviour.BehaveOnUpdate();
        }
    }
}
