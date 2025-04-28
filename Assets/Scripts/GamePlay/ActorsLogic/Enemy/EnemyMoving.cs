using UnityEngine;

public class EnemyMoving : Enemy
{
    private bool isFirstTimeInitiated = true;
    private ActorBehaviour actorBehaviour;

    public AnimalAnimationController animationController;

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
    }

    private void RegisterEvents()
    {
        GameManager.Instance.OnTimeToggle += OnTimeToggle;
        GameManager.Instance.OnQuitGameScene += ReleaseEvents;
    }

    private void ReleaseEvents()
    {
        GameManager.Instance.OnTimeToggle -= OnTimeToggle;
        GameManager.Instance.OnQuitGameScene -= ReleaseEvents;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // factory 제작 및 initiating 이후에 수정해야 함.
        Init(targetEnemyDataSO);
    }

    // Update is called once per frame
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
