using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private Player owner;
    public PlayerStat stat;

    [Header("조이스틱 부분")]
    public VariableJoystick variableJoystick;
    public float joystickForceToChargeBoost = 0.8f;

    public LayerMask groundLayer;   // Ground 레이어 지정

    public Transform cameraTransform; // 카메라의 Transform
    private Vector3 cameraOffset;

    public Transform playerTransform;
    public Rigidbody playerRB;

    // 시간 정지 부분
    public bool IsTimeStopped = false;
    private Vector3 storedLinearVelocity;
    private Vector3 storedAngularVelocity;

    private PlayerStabilityChecker stabilityChecker;


    [Header("충돌 부분")]
    private PlayerCollision playerCollision;

    private Camera mainCamera;

    public void Init(Player player)
    {
        mainCamera = Camera.main;
        owner = player;
        stat = player.Stat;
        playerTransform = owner.PlayerTransform;
        playerRB = playerTransform.GetComponent<Rigidbody>();
        playerCollision = playerTransform.GetComponent<PlayerCollision>();
        playerCollision.Init(this);
        stabilityChecker = GetComponent<PlayerStabilityChecker>();
        cameraOffset = cameraTransform.localPosition;
        RegisterEvent();
    }

    private void RegisterEvent()
    {
        GameManager.Instance.OnTimeToggle += OnTimeToggle;
        GameManager.Instance.OnQuitGameScene += ReleaseEvent;
    }

    private void ReleaseEvent()
    {
        GameManager.Instance.OnTimeToggle -= OnTimeToggle;
        GameManager.Instance.OnQuitGameScene -= ReleaseEvent;
    }

    void FixedUpdate()
    {
        if (IsTimeStopped) return;

        CalculateAdditionalForce(stabilityChecker.CheckBoostEnabled(playerTransform));
        HandleMovement();

        HandleRotation();
        CalculateSpeed();
        owner.animationController.ChangeAnimationDependOnSpeed();
    }

    void Update()
    {
        if (IsTimeStopped) return;

        HandleBoost();
        HandleStatChange();
        
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void OnTimeToggle(bool isTimeStop)
    {
        IsTimeStopped = isTimeStop;

        if (isTimeStop)
        {
            // 시간 정지 전 속도 저장
            storedLinearVelocity = playerRB.linearVelocity;
            storedAngularVelocity = playerRB.angularVelocity;

            // 물리 시뮬레이션 비활성화
            playerRB.isKinematic = true;
        }
        else
        {
            // 물리 시뮬레이션 재개
            playerRB.isKinematic = false;

            // 저장된 속도 복구
            playerRB.linearVelocity = storedLinearVelocity;
            playerRB.angularVelocity = storedAngularVelocity;
        }
    }


    /// <summary>
    /// 카메라 위치 조정
    /// </summary>
    private void UpdateCameraPosition()
    {
        if (cameraTransform != null)
        {
            cameraTransform.position = playerTransform.position + cameraOffset;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="boostEnabled"></param>
    /// <returns></returns>
    private float CalculateAdditionalForce(bool boostEnabled)
    {
        if (!boostEnabled)
        {
            stat.CurrentAdditionForce = 0f;
        }

        stat.CurrentAdditionForce += stat.AdditionForceRatio * Time.deltaTime;
        if (stat.CurrentAdditionForce > stat.AdditionForceMax)
        {
            stat.CurrentAdditionForce = stat.AdditionForceMax;
        }
        return stat.CurrentAdditionForce;
    }
    void HandleMovement()
    {
        if(!owner.feverHandler.isFever)
        {
            HandleNormalMovement();
        }
        else
        {
            HandleFeverMovement();
        }
    }

    private void HandleNormalMovement()
    {
        playerRB.AddForce(playerTransform.forward * variableJoystick.Strength * (stat.MoveForce + stat.CurrentAdditionForce), ForceMode.Force);
    }

    private void HandleFeverMovement()
    {
        playerRB.AddForce(playerTransform.forward * variableJoystick.Strength * (stat.MoveForce + stat.CurrentAdditionForce), ForceMode.Force);
    }

    void HandleRotation()
    {
        HandleRotationOnJoyStick();
    }

    private void HandleRotationOnJoyStick()
    {
        
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        if(direction !=  Vector3.zero)
        {
            Quaternion wanderRotation = Quaternion.LookRotation(direction);
            playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, wanderRotation, stat.RotationSpeed * Time.deltaTime);
        }
    }

    private void HandleRotationOnPCOld()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;

            Vector3 direction = targetPosition - playerTransform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion wanderRotation = Quaternion.LookRotation(direction);
                playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, wanderRotation, stat.RotationSpeed * Time.deltaTime);
            }
        }
    }

    void HandleBoost()
    {
        ChargeBoost();

        // 캐주얼함을 늘리기 위해 자동으로 부스터 수정.
        if(stat.BoostChargeRatio >= 1.0f)
        {
            playerRB.AddForce(playerTransform.forward * stat.BoostForce, ForceMode.Impulse);
            SoundManager.Instance.PlaySFX("Booster", AudioType.Entity);
            owner.particleController.OnBoost();
            stat.LastDashTime = Time.time;
            stat.BoostChargeRatio = 0f;
        }
    }

    private void ChargeBoost()
    {
        if (!owner.feverHandler.isFever)
        {
            if (variableJoystick.Strength >= joystickForceToChargeBoost)
            {
                ChargeBoost(stat.BoostChargeTime);
            }
        }
        else
        {
            ChargeBoost(stat.BoostFeverChargeTime);
        }
    }

    private void ChargeBoost(float chargeTime)
    {
        float ratioDelta = Time.deltaTime / chargeTime;
        stat.BoostChargeRatio += ratioDelta;
        if (stat.BoostChargeRatio > 1)
        {
            stat.BoostChargeRatio = 1;
        }
    }


    private void CalculateSpeed()
    {
        stat.RigidbodySpeed = playerRB.linearVelocity.magnitude;
    }

    public float CalculateImpulseDamage(float impulseMagnitude, bool isCritical)
    {
        return stat.CalculateDamage(impulseMagnitude, isCritical);

    }

    private void HandleStatChange()
    {
        playerTransform.localScale = new Vector3(owner.Stat.CurrentSize, owner.Stat.CurrentSize, owner.Stat.CurrentSize);
        playerRB.mass = owner.Stat.CurrentMass;
    }

}
