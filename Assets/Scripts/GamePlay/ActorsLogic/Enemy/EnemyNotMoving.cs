using UnityEngine;

public class EnemyNotMoving : Enemy
{
    // start���� Init���� ���Ŀ� �ű��.
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
        // factory ���� �� initiating ���Ŀ� �����ؾ� ��.
        Init(targetEnemyDataSO);
    }
}
