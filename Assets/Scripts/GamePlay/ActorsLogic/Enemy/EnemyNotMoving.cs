using Unity.VisualScripting;
using UnityEngine;

public class EnemyNotMoving : Enemy
{
    // start에서 Init으로 추후에 옮기기.
    public override void Init(EnemyDataSO enemyData)
    {
        ResetStates();
        EnemyRB = EnemyTransform.GetComponent<Rigidbody>();
        targetEnemyDataSO = enemyData;
        stat.CopyData(enemyData.ActorsStat);

        actorPhysics = GetComponent<ActorCollision>();
        actorPhysics.Init(this, EnemyTransform);
        InitializeModel();

        OnInit?.Invoke();
    }

    private void Update()
    {
        if (GameManager.Instance.IsTimeStop) return;

    }

}
