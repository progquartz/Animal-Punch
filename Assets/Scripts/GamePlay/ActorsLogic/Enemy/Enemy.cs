using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyDataSO targetEnemyDataSO;
    public EnemyStat stat;

    public ActorCollision actorPhysics;
    public Transform EnemyTransform;
    public Rigidbody EnemyRB;

    
    // start���� Init���� ���Ŀ� �ű��.
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
        // factory ���� �� initiating ���Ŀ� �����ؾ� ��.
        Init(targetEnemyDataSO);
    }

    void Update()
    {
    }
}
