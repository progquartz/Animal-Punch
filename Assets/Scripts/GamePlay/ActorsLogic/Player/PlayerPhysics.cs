using Mono.Cecil.Cil;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private PlayerStat stat;

    public LayerMask groundLayer;   // Ground 레이어 지정

    public Transform cameraTransform; // 카메라의 Transform
    private Vector3 cameraOffset;

    [Header("물리 부분")]
    public Transform playerTransform;
    private Rigidbody playerRB;
    
    private PlayerStabilityChecker stabilityChecker;

    [Header("충돌 부분")]
    private PlayerCollision playerCollision;

    private Camera mainCamera;

    public void Init(Player player)
    {
        stat = player.stat;
        mainCamera = Camera.main;
        playerRB = playerTransform.GetComponent<Rigidbody>();
        playerCollision = playerTransform.GetComponent<PlayerCollision>();
        stabilityChecker = GetComponent<PlayerStabilityChecker>();
        cameraOffset = cameraTransform.localPosition;
    }


    void FixedUpdate()
    {
        CalculateBoosterSpeed(stabilityChecker.CheckBoostEnabled(playerTransform));
        HandleMovement();
        CalculateSpeed();
    }

    void Update()
    {
        HandleRotation();
        HandleDash();


        if (cameraTransform != null)
        {
            cameraTransform.position = playerTransform.position + cameraOffset;
        }
    }

    private float CalculateBoosterSpeed(bool boostEnabled)
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
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation, stat.RotationSpeed * Time.deltaTime);
            }
        }
    }

    void HandleDash()
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
}
