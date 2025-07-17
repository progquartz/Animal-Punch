using UnityEngine;

public class PlayerModelPlaceHolder : MonoBehaviour
{
    [Header("모델 부모 Transform")]
    public Transform modelParent; // 자식 중 회전시킬 모델이 들어있음

    public string modelKey;


    [Header("회전 민감도")]
    public float rotationSensitivity = 0.2f;

    public GameObject currentModel;
    private Camera mainCamera;

    private bool isDragging = false;

    private Quaternion initialRotation = Quaternion.Euler(Vector3.zero);
    private float timeSinceLastTouch;
    private float returnRotationTime = 0.4f;
    private float returnRotationSpeed = 10f;

    private Vector2 lastTouchPos;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateCurrentModel();
        HandleTouchInput();
    }

    public void ToggleModel(bool state)
    {
        UpdateCurrentModel();
        if (state)
        {
            currentModel.SetActive(true);
        }
        else
        {
            currentModel.transform.localRotation = initialRotation;
            currentModel.SetActive(false);
        }
    }

    private void UpdateCurrentModel()
    {
        string managerModelKey = UnlockSaveManager.Instance.selectedIds;

        if (modelKey != managerModelKey)
        {
            modelKey = managerModelKey;
            ChangeModel(managerModelKey);
            return;
        }
        if(currentModel == null)
        {
            // 만약에 모델이 나오지 않는다면, 기본값으로 소환
            ChangeModel("Cheetah");
        }
    }

    private void ChangeModel(string modelKey)
    {
        UnlockableDataSO data = UnlockSaveManager.Instance.GetUnlockdata(modelKey);
        if (data == null) return;

        for(int i = 0; i < modelParent.childCount; i++)
        {
            Destroy(modelParent.GetChild(0).gameObject);
        }
        var quality = SettingsManager.Instance.graphicsQuality;
        GameObject model = Instantiate(data.GetModelByQuality(quality), modelParent.transform);
        currentModel = model;
        AnimalAnimationController controller = model.AddComponent<AnimalAnimationController>();
        controller.SetAnimator(model.GetComponent<Animator>());
        controller.ChangeAnimation(AnimalAnimation.Run);
        model.GetComponent<CapsuleCollider>().enabled = true;
        model.SetActive(false);
    }

    private void HandleTouchInput()
    {
        CheckReturningOriginalRotation();

#if UNITY_EDITOR
        HandlePCInput();
#else
        HandleMobileInput();
#endif
    }

    private void CheckReturningOriginalRotation()
    {
        if (!isDragging)
        {
            if (timeSinceLastTouch >= returnRotationTime)
            {
                currentModel.transform.localRotation = Quaternion.Lerp(
                    currentModel.transform.localRotation,
                    initialRotation,
                    Time.deltaTime * returnRotationSpeed
                );
            }
            else
            {
                timeSinceLastTouch += Time.deltaTime;
            }
        }
    }

    private void HandleMobileInput()
    {
        // 모바일 터치 처리
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsModelTouched(touch.position))
                    {
                        isDragging = true;
                        lastTouchPos = touch.position;
                    }
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        float deltaX = touch.position.x - lastTouchPos.x;
                        RotateModel(deltaX);
                        lastTouchPos = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }

    }

    private void HandlePCInput()
    {
        // PC 처리
        if (Input.GetMouseButtonDown(0))
        {

            if (IsModelTouched(Input.mousePosition))
            {
                Debug.Log("마우스클릭됨");
                isDragging = true;
                lastTouchPos = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 currentPos = Input.mousePosition;
            float deltaX = currentPos.x - lastTouchPos.x;
            RotateModel(deltaX);
            lastTouchPos = currentPos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private bool IsModelTouched(Vector2 screenPos)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.transform == currentModel.transform;
        }
        return false;
    }

    private void RotateModel(float deltaX)
    {
        if (currentModel != null)
        {
            float rotationY = deltaX * rotationSensitivity;
            currentModel.transform.Rotate(Vector3.up, -rotationY, Space.World);
        }
    }
}
