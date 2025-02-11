using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float acceleration = 0.5f;
    public float maxSpeed = 10f;
    public float turnSpeed = 20f;
    public float deceleration = 0.3f;
    public float stopSpeedThreshold = 0.1f;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private bool isAiming = false;

    public Camera mainCamera; // Основная камера
    public Camera aimingCamera; // Камера для режима прицеливания
    public float cameraRotationSpeed = 2f; // Скорость вращения камеры
    public float maxCameraAngle = 30f; // Максимальный угол наклона камеры
    public float maxYawAngle = 60f; // Максимальный угол поворота камеры (по горизонтали)

    private float cameraPitch = 0f; // Угол наклона камеры
    private float cameraYaw = 100f; // Угол поворота камеры

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1f;
        rb.angularDamping = 3f;

        if (mainCamera == null || aimingCamera == null)
        {
            Debug.LogError("Обе камеры должны быть установлены в ShipController!");
        }

        aimingCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;
            Debug.Log("Режим прицеливания: " + isAiming);

            mainCamera.gameObject.SetActive(!isAiming);
            aimingCamera.gameObject.SetActive(isAiming);
        }

        HandleMovement();

        if (isAiming)
        {
            HandleCameraRotation();
        }
    }

    void FixedUpdate()
    {
        if (!isAiming)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        if (moveInput > 0 && currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.fixedDeltaTime;
        }
        else if (moveInput < 0 && currentSpeed > -maxSpeed)
        {
            currentSpeed -= acceleration * Time.fixedDeltaTime;
        }
        else
        {
            if (currentSpeed > stopSpeedThreshold)
            {
                currentSpeed -= deceleration * Time.fixedDeltaTime;
            }
            else if (currentSpeed < -stopSpeedThreshold)
            {
                currentSpeed += deceleration * Time.fixedDeltaTime;
            }
            else
            {
                currentSpeed = 0;
            }
        }

        rb.linearVelocity = -transform.forward * currentSpeed;

        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float turnAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraRotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * cameraRotationSpeed;

        cameraYaw += mouseX;
        cameraPitch -= mouseY;

        // Ограничиваем угол наклона (вверх-вниз)
        cameraPitch = Mathf.Clamp(cameraPitch, -maxCameraAngle, maxCameraAngle);

        // Ограничиваем угол поворота (влево-вправо)
        cameraYaw = Mathf.Clamp(cameraYaw, -maxYawAngle, maxYawAngle);

        aimingCamera.transform.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
    }
}
