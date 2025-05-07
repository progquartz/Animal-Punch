using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour
{
    public EnemyDataSO targetEnemyDataSO;
    public EnemyStat stat;

    public GameObject ModelGameObject;

    public ActorCollision actorPhysics;
    public Transform EnemyTransform;
    public Rigidbody EnemyRB;

    public Action OnDead;
    public Action OnInit;

    public bool IsInPool = false;

    
    // start에서 Init으로 추후에 옮기기.
    public virtual void Init(EnemyDataSO enemyData)
    {
        EnemyRB = EnemyTransform.GetComponent<Rigidbody>();
        EnemyTransform.parent = MapManager.Instance.EnemySpawner.EnemyParentTransform;
        targetEnemyDataSO = enemyData;
        stat.CopyData(enemyData.ActorsStat);

        actorPhysics = GetComponent<ActorCollision>();
        actorPhysics.Init(this, EnemyTransform);
        
        OnInit?.Invoke();
    }


    public Animator InitializeModel()
    {
        if (ModelGameObject == null)
        {
            ModelGameObject = Instantiate(targetEnemyDataSO.ModelObject, transform);
        }
        return ModelGameObject.GetComponent<Animator>();
    }

    public virtual void HandleDeath()
    {
        stat.IsDead = true;
        
        LootingManager.Instance.DropLoot(targetEnemyDataSO.DropItemData, EnemyTransform.position);
        OnDead?.Invoke();
    }

    protected void ResetStates()
    {
        stat.IsDead = false;
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
