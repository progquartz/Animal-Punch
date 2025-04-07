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
    }

    void FixedUpdate()
    {
        CalculateAdditionalForce(stabilityChecker.CheckBoostEnabled(playerTransform));
        HandleMovement();
        CalculateSpeed();
    }

    void Update()
    {
        HandleRotation();
        HandleBoost();
        UpdateCameraPosition();
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
        if(!boostEnabled)
        {
            stat.CurrentAdditionForce = 0f;
        }

        stat.CurrentAdditionForce += stat.AdditionForceRatio * Time.deltaTime;
        if(stat.CurrentAdditionForce > stat.AdditionForceMax)
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
        float baseDamage = stat.CollisionDamageBase + stat.CollisionDamageAdditional;
        float impulseDamage = stat.CollisionImpulseDamageBase * (impulseMagnitude / stat.CollisionImpulseStandard) * stat.CollisionImpulseDamageRatio;
        Debug.Log($"데미지 = {baseDamage + impulseDamage} / 기본 데미지 = {baseDamage} / 충격량 데미지 = {impulseDamage}");
        return baseDamage + impulseDamage;
    }
}
