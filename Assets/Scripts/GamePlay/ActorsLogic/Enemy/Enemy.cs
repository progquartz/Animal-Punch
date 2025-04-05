using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyDataSO targetEnemyDataSO;
    public ActorsStat stat;

    public ActorCollision actorPhysics;
    [SerializeField] private Transform enemyTransform;
    private ActorBehaviour actorBehaviour;
    
    // start���� Init���� ���Ŀ� �ű��.
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
        // factory ���� �� initiating ���Ŀ� �����ؾ� ��.
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
