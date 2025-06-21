using System;
using System.Collections;
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
        
        if (GameManager.Instance.IsGamePaused)
        {
            EnemyRB.isKinematic = true;
            EnemyRB.linearVelocity = Vector3.zero;
            EnemyRB.Sleep();
        }

        targetEnemyDataSO = enemyData;
        stat.CopyData(enemyData.ActorsStat);
        stat.InitializeGameTimeScale();

        actorPhysics = GetComponent<ActorCollision>();
        actorPhysics.Init(this, EnemyTransform);
        OnInit?.Invoke();
    }



    public Animator InitializeModel()
    {
        if (ModelGameObject == null)
        {
            var quality = SettingsManager.Instance.graphicsQuality;
            GameObject prefab = targetEnemyDataSO.GetModelByQuality(quality);
            ModelGameObject = Instantiate(prefab, transform);
            ModelGameObject.transform.localPosition = Vector3.zero;
            ModelGameObject.transform.localRotation = Quaternion.identity;
        }
        return ModelGameObject.GetComponent<Animator>();
    }

    public virtual void HandleDeath()
    {
        stat.IsDead = true;
        
        LootingManager.Instance.DropLoot(targetEnemyDataSO.DropItemData, EnemyTransform.position);
        OnDead?.Invoke();

        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath(targetEnemyDataSO.ActorKey);
    }
    public virtual bool HandleDamage(Collision collision, float impulseDamage, bool isCritical)
    {
        bool isDead = stat.HandleDamage(impulseDamage);
        return isDead;
    }

    protected void ResetStates()
    {
        stat.IsDead = false;
    }


}
