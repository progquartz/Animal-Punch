using System.Collections;
using UnityEngine;

public class EnemyMoving : Enemy
{
    private bool isFirstTimeInitiated = true;
    private ActorBehaviour actorBehaviour;

    public AnimalAnimationController animationController;
    public EnemyParticleController particleController;

    // start에서 Init으로 추후에 옮기기.
    public override void Init(EnemyDataSO enemyData ,Vector3 randomPos)
    {
        base.Init(enemyData, randomPos);
        if(isFirstTimeInitiated)
        {
            isFirstTimeInitiated = false;
            RegisterEvents();
        }
        ResetStates();
      
        actorBehaviour = ActorBehaviour.GetActorBehaviour(targetEnemyDataSO.BehaviourType);
        actorBehaviour.Init(this);
        animationController.SetAnimator(InitializeModel());
        animationController.Init();
        particleController.Init(this);
        StartCoroutine(LatePositionFix(randomPos));
    }

    protected IEnumerator LatePositionFix(Vector3 randomPos)
    {
        yield return null;
        transform.parent = MapManager.Instance.EnemySpawner.EnemyParentTransform;
        transform.position = randomPos;
        EnemyTransform.position = randomPos;
    }



    public override void HandleDeath()
    {
        stat.IsDead = true;
        particleController.OnDead();
        animationController.OnDead();
        LootingManager.Instance.DropLoot(targetEnemyDataSO.DropItemData, EnemyTransform.position);
        OnDead?.Invoke();
    }

    private void RegisterEvents()
    {
        GameManager.Instance.OnTimeToggle += OnTimeToggle;
        GameManager.Instance.OnTimeToggle += particleController.OnTimeToggle;
        GameManager.Instance.OnQuitGameScene += ReleaseEvents;
        
    }

    private void ReleaseEvents()
    {
        GameManager.Instance.OnTimeToggle -= OnTimeToggle;
        GameManager.Instance.OnTimeToggle -= particleController.OnTimeToggle;   
        GameManager.Instance.OnQuitGameScene -= ReleaseEvents; // 가장 마지막에 적용되어야 함.
    }
    
    void Update()
    {
        if (GameManager.Instance.IsTimeStop) return;
        
        if (targetEnemyDataSO.IsEnemyHasCondition)
        {
            actorBehaviour.CheckCondition();
            actorBehaviour.BehaveOnUpdate();
        }
    }

    private void OnTimeToggle(bool isTimeStop)
    {
        if(isTimeStop)
        {
            EnemyRB.isKinematic = true;
            animationController.PauseAnimation();
        }
        else
        {
            EnemyRB.isKinematic = false;
            animationController.ResumeAnimation();
        }
    }

    public override void OnDamage(bool IsCritical)
    {
        if(IsCritical)
        {
            particleController.OnCriticalHit();
        }
        else
        {
            particleController.OnHit();
        }
        
    }




}
