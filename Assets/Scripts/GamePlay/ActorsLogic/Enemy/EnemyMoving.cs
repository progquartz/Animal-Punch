using UnityEngine;

public class EnemyMoving : Enemy
{
    private bool isFirstTimeInitiated = true;
    private ActorBehaviour actorBehaviour;

    public AnimalAnimationController animationController;
    public EnemyParticleController particleController;

    public override void Init(EnemyDataSO enemyData, Vector3 spawnPosition)
    {
        base.Init(enemyData, spawnPosition);

        transform.position = spawnPosition;
        EnemyTransform.position = spawnPosition;

        if (isFirstTimeInitiated)
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
    }

    public override void HandleDeath()
    {
        stat.IsDead = true;
        particleController.OnDead();
        animationController.OnDead();

        LootingManager.Instance.DropLoot(targetEnemyDataSO.DropItemData, EnemyTransform.position);
        PlayDeadSound();
        OnDead?.Invoke();
    }

    private void PlayDeadSound()
    {
        SoundManager.Instance.PlaySFX("EnemyShooting");
        SoundManager.Instance.PlaySFX(targetEnemyDataSO.ActorKey + "Dead");
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
        GameManager.Instance.OnQuitGameScene -= ReleaseEvents;
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
        if (isTimeStop)
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
        if (IsCritical) particleController.OnCriticalHit();
        else particleController.OnHit();
    }
}
