using UnityEngine;

public class EnemyMoving : Enemy
{
    private bool isFirstTimeInitiated = true;
    private ActorBehaviour actorBehaviour;

    public AnimalAnimationController animationController;
    public EnemyParticleController particleController;

    private Vector3 storedLinearVelocity;
    private Vector3 storedAngularVelocity;

    // start에서 Init으로 추후에 옮기기.
    public override void Init(EnemyDataSO enemyData)
    {
        base.Init(enemyData);
        if(isFirstTimeInitiated)
        {
            isFirstTimeInitiated = false;
            RegisterEvents();
        }
        ResetStates();
      
        actorBehaviour = ActorBehaviour.GetActorBehaviour(stat.BehaviourType);
        actorBehaviour.Init(this);
        particleController.Init(this);
    }

    public override void HandleDeath()
    {
        stat.IsDead = true;
        particleController.OnDead();
        LootingManager.Instance.DropLoot(false, EnemyTransform.position);
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

    void Start()
    {
        // factory 제작 및 initiating 이후에 수정해야 함.
        Init(targetEnemyDataSO);
    }

    
    void Update()
    {
        if (GameManager.Instance.IsTimeStop) return;
        
        if (stat.IsEnemyHasCondition)
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


}
