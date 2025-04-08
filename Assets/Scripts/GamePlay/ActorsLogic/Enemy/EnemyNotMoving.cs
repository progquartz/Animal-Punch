using UnityEngine;

public class EnemyNotMoving : Enemy
{
    // start에서 Init으로 추후에 옮기기.
    public override void Init(EnemyDataSO enemyData)
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
}
