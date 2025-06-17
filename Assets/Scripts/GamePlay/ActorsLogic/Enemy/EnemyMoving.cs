using UnityEngine;

public class EnemyMoving : Enemy
{
    private bool isFirstTimeInitiated = true;
    private ActorBehaviour actorBehaviour;

    public AnimalAnimationController animationController;
    public EnemyParticleController particleController;
    private Vector3 PausedVelocity = Vector3.zero;

    public override void Init(EnemyDataSO enemyData)
    {
        base.Init(enemyData);

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
        SoundManager.Instance.PlaySFX("EnemyShooting", AudioType.Entity);
        SoundManager.Instance.PlaySFX(targetEnemyDataSO.ActorKey + "Dead", AudioType.Entity);
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

    void FixedUpdate()
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

        if (EnemyRB == null) return;

        if (isTimeStop)
        {
            // 현재 속도 저장
            PausedVelocity = EnemyRB.linearVelocity;

            // Rigidbody를 멈추고, 물리 연산 비활성화
            EnemyRB.linearVelocity = Vector3.zero;
            EnemyRB.angularVelocity = Vector3.zero;
            EnemyRB.isKinematic = true;
            EnemyRB.Sleep();

            // 애니메이션 멈춤
            animationController.PauseAnimation();
        }
        else
        {
            // Kinematic 해제 전 위치 강제 갱신 → 물리 보정 최소화
            EnemyRB.position += Vector3.zero;
            EnemyRB.WakeUp();

            EnemyRB.isKinematic = false;

            // 저장된 속도 복원
            EnemyRB.linearVelocity = PausedVelocity;
            PausedVelocity = Vector3.zero;

            // 애니메이션 재생
            animationController.ResumeAnimation();
        }
    }

    public override bool HandleDamage(Collision collision, float impulseDamage, bool isCritical)
    {
        bool isDead = base.HandleDamage(collision, impulseDamage, isCritical);
        HandleDamageVisual(collision, impulseDamage, isCritical);
        HandleDamageSound(collision, isCritical);
        
        if(isDead)
        {
            HandleDeath();
        }
        return isDead;
    }

    private void HandleDamageVisual(Collision collision, float impulseDamage, bool isCritical)
    {
        // damage indicator text
        Color textColor;
        if (isCritical)
            textColor = Color.red;
        else
            textColor = Color.white;

        InGameTextPooler.Instance.SpawnText(((int)impulseDamage).ToString(), textColor, EnemyTransform.position);

        // particle
        if (isCritical) particleController.OnCriticalHit();
        else particleController.OnHit();

    }

    private void HandleDamageSound(Collision collision, bool isCritical)
    {
        SoundManager.Instance.PlaySFX("EnemyHit", AudioType.SFX, EnemyTransform.position);
    }
}
