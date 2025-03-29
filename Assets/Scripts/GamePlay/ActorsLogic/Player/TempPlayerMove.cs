using Mono.Cecil.Cil;
using UnityEngine;

public class TempPlayerMove : MonoBehaviour
{

    public LayerMask groundLayer;   // Ground 레이어 지정

    public Transform cameraTransform; // 카메라의 Transform
    private Vector3 cameraOffset;

    [Header("물리 부분")]
    public Transform playerTransform;
    private Rigidbody playerRB;

    [Header("대쉬 부문")]
    public float dashForce = 15f;               // 앞으로 튀어나갈 힘
    public float spaceCooltime = 2f;            // Space 키 쿨타임

    private float lastDashTime;

    [Header("이동 부문")]
    public float RigidbodySpeed = 0f; // 속도
    public float rotationSpeed = 1000f;          // 최대 회전 속도.
    private PlayerStabilityChecker stabilityChecker;

    public float moveForce = 10f;                // 이동할 때 가해지는 힘
    public float currentBoostForce = 0f;       // 현재 가속도

    public float boosterAcceleration = 2f;
    public float maxBoostForce = 5f;
    
    

    


    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        playerRB = playerTransform.GetComponent<Rigidbody>();
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
            currentBoostForce = 0f;
        }

        currentBoostForce += boosterAcceleration * Time.deltaTime;
        if(currentBoostForce > maxBoostForce)
        {
            currentBoostForce = maxBoostForce;
        }
        return currentBoostForce;
    }
    void HandleMovement()
    {
        playerRB.AddForce(playerTransform.forward * (moveForce + currentBoostForce), ForceMode.Force);
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
                playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + spaceCooltime)
        {
            playerRB.AddForce(playerTransform.forward * dashForce, ForceMode.Impulse);
            lastDashTime = Time.time;
        }
    }

    private void CalculateSpeed()
    {
        RigidbodySpeed = playerRB.linearVelocity.magnitude;
    }
}
