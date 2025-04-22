using Mono.Cecil.Cil;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private Player owner;
    private PlayerStat stat;

    public LayerMask groundLayer;   // Ground 레이어 지정

    public Transform cameraTransform; // 카메라의 Transform
    private Vector3 cameraOffset;

    [HideInInspector]
    public Transform playerTransform;
    public Rigidbody playerRB { get; private set; }

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
    }

    private void ReleaseEvent()
    {
        GameManager.Instance.OnTimeToggle -= OnTimeToggle;
    }

    void FixedUpdate()
    {
        if (IsTimeStopped) return;

        CalculateAdditionalForce(stabilityChecker.CheckBoostEnabled(playerTransform));
        HandleMovement();
        CalculateSpeed();
    }

    void Update()
    {
        if (IsTimeStopped) return;

        HandleRotation();
        HandleBoost();
        HandleStatChange();
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
        playerRB.AddForce(playerTransform.forward * (stat.MoveForce + stat.CurrentAdditionForce), ForceMode.Force);
    }

    void HandleRotation()
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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= stat.LastDashTime + stat.BoostCooltime)
        {
            playerRB.AddForce(playerTransform.forward * stat.BoostForce, ForceMode.Impulse);
            stat.LastDashTime = Time.time;
        }
    }

    private void CalculateSpeed()
    {
        stat.RigidbodySpeed = playerRB.linearVelocity.magnitude;
    }

    public float CalculateImpulseDamage(float impulseMagnitude)
    {
        return stat.CalculateDamage(impulseMagnitude);

    }

    private void HandleStatChange()
    {
        playerTransform.localScale = new Vector3(owner.Stat.CurrentSize, owner.Stat.CurrentSize, owner.Stat.CurrentSize);
        playerRB.mass = owner.Stat.CurrentMass;
    }

}
