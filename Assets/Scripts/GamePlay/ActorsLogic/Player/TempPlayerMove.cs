using UnityEngine;

public class TempPlayerMove : MonoBehaviour
{

    public LayerMask groundLayer;   // Ground 레이어 지정

    public Transform cameraTransform; // 카메라의 Transform
    private Vector3 cameraOffset;

    public Transform playerTransform;
    private Rigidbody playerRB;
    private float lastDashTime;

    public float dashForce = 15f;               // 앞으로 튀어나갈 힘
    public float spaceCooltime = 2f;            // Space 키 쿨타임

    public float moveForce = 10f;                // 이동할 때 가해지는 힘
    public float maxSpeed = 5f;                  // 최대 이동 속도
    public float rotationSpeed = 1000f; // 회전 속도 제한 (degrees per second)


    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        playerRB = playerTransform.GetComponent<Rigidbody>();
        cameraOffset = cameraTransform.localPosition;
    }


    void FixedUpdate()
    {
        HandleMovement();
        //LimitSpeed();
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

    void HandleMovement()
    {
        playerRB.AddForce(playerTransform.forward * moveForce, ForceMode.Force);
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

    void LimitSpeed()
    {
        Vector3 horizontalVelocity = playerRB.linearVelocity;
        horizontalVelocity.y = 0f;

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = horizontalVelocity.normalized * maxSpeed;
            playerRB.linearVelocity = new Vector3(limitedVelocity.x, playerRB.linearVelocity.y, limitedVelocity.z);
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
}
