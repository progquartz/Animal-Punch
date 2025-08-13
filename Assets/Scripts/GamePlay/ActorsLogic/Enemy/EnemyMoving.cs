using UnityEngine;

public class EnemyMoving : Enemy
{
    private bool isFirstTimeInitiated = true;
    private ActorBehaviour actorBehaviour;

    public AnimalAnimationController animationController;
    public EnemyParticleController particleController;
    private Vector3 PausedVelocity = Vector3.zero;


    private float renderDistance = 50f; // 플레이어 기준 거리
    private float renderBuffer = 5f;    // 깜빡임 방지용 버퍼
    private bool isCurrentlyVisible = true;

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
        if (GameManager.Instance.IsGamePaused) return;

        if (targetEnemyDataSO.IsEnemyHasCondition)
        {
            actorBehaviour.CheckCondition();
            actorBehaviour.BehaveOnUpdate();
        }
    }

    void LateUpdate()
    {
        HandleDistanceBasedRendering();
    }

    private void HandleDistanceBasedRendering()
    {
        if (ModelGameObject == null) return;

        Vector3 playerPos = Player.Instance.PlayerTransform.position;
        float distanceToPlayer = Vector3.Distance(playerPos, EnemyTransform.position);

        bool shouldBeVisible = distanceToPlayer <= renderDistance;

        // 버퍼 거리 도입하여 깜빡임 방지 (히스테리시스 방식)
        if (!isCurrentlyVisible && distanceToPlayer < renderDistance - renderBuffer)
        {
            shouldBeVisible = true;
        }
        else if (isCurrentlyVisible && distanceToPlayer > renderDistance + renderBuffer)
        {
            shouldBeVisible = false;
        }

        if (shouldBeVisible != isCurrentlyVisible)
        {
            ModelGameObject.SetActive(shouldBeVisible);
            isCurrentlyVisible = shouldBeVisible;
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

    public override void HandleDeath()
    {
        base.HandleDeath();
        particleController.OnDead();
        animationController.OnDead();

        DropManager.Instance.DropLoot(targetEnemyDataSO.DropItemData, EnemyTransform.position);
        GameManager.Instance.EnemyDeathCountHandler.RegisterDeath(targetEnemyDataSO.ActorKey);
        PlayDeadSound();
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

    private void PlayDeadSound()
    {
        SoundManager.Instance.PlaySFX("EnemyShooting", AudioType.Entity);
        SoundManager.Instance.PlaySFX(targetEnemyDataSO.ActorKey + "Dead", AudioType.Entity);
    }

}
