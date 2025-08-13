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

    // 다운포스 부분
    private float downforceAmount = 10f; 

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
        Debug.Log("Register");
        GameManager.Instance.OnTimeToggle += OnTimeToggle;
        GameManager.Instance.OnQuitGameScene += ReleaseEvent;
    }

    private void ReleaseEvent()
    {
        Debug.Log("Release");
        GameManager.Instance.OnTimeToggle -= OnTimeToggle;
        GameManager.Instance.OnQuitGameScene -= ReleaseEvent;
    }

    void FixedUpdate()
    {
        if (IsTimeStopped) return;

        CalculateAdditionalForce(stabilityChecker.CheckBoostEnabled(playerTransform));
        HandleMovement();
        HandleDownForce();
        HandleRotation();
        CalculateSpeed();
        owner.animationController.ChangeAnimationDependOnSpeed();
    }

    void Update()
    {
        if (IsTimeStopped) return;

        UpdateForDash();
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

    private void HandleDownForce()
    {
        playerRB.AddForce(Vector3.down.normalized * downforceAmount * Time.deltaTime);
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

    private void UpdateForDash()
    {
        UpdateDashRatio();
        owner.OnDash?.Invoke();
    }

    public void Dash()
    {
        if (stat.DashChargeRatio >= 1.0f)
        {
            playerRB.AddForce(playerTransform.forward * stat.DashForce, ForceMode.Impulse);
            SoundManager.Instance.PlaySFX("Booster", AudioType.Entity);
            owner.particleController.OnBoost();
            stat.LastDashTime = Time.time;
            stat.DashChargeRatio = 0f;
        }
    }

    private void UpdateDashRatio()
    {
        if (!owner.feverHandler.isFever)
        {
            if (variableJoystick.Strength >= joystickForceToChargeBoost)
            {
                // 피버 상태가 아닌 경우
                ChargeDash(stat.DashChargeTime);
            }
        }
        else
        {
            // 피버 상태일 경우
            ChargeDash(stat.FeverBoostChargeTime);
        }
    }

    private void ChargeDash(float chargeTime)
    {
        float ratioDelta = Time.deltaTime / chargeTime;
        stat.DashChargeRatio += ratioDelta;
        if (stat.DashChargeRatio > 1)
        {
            stat.DashChargeRatio = 1;
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
