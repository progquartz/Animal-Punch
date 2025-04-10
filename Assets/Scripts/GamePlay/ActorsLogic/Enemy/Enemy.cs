using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyDataSO targetEnemyDataSO;
    public EnemyStat stat;

    public ActorCollision actorPhysics;
    public Transform EnemyTransform;
    public Rigidbody EnemyRB;

    
    // start에서 Init으로 추후에 옮기기.
    public virtual void Init(EnemyDataSO enemyData)
    {
        EnemyRB = EnemyTransform.GetComponent<Rigidbody>();
        targetEnemyDataSO = enemyData;
        stat.CopyData(enemyData.ActorsStat);

        actorPhysics = GetComponent<ActorCollision>();
        actorPhysics.Init(this, EnemyTransform);
    }

    void Start()
    {
        // factory 제작 및 initiating 이후에 수정해야 함.
        Init(targetEnemyDataSO);
    }

    void Update()
    {
    }
}
