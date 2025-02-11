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

    public Camera mainCamera;
    public Camera aimingCamera;
    public Transform shootPoint;
    public float cameraRotationSpeed = 2f;
    public float maxCameraAngle = 30f;
    public float maxYawAngle = 60f;

    private float cameraPitch = 0f;
    private float cameraYaw = 100f;

    public float shootRange = 100f;
    public LayerMask shootableLayer;
    public GameObject hitEffectPrefab;
    public GameObject muzzleFlashPrefab;
    public AudioSource shootSound;
    public AudioSource explosionSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1f;
        rb.angularDamping = 3f;

        if (mainCamera == null || aimingCamera == null)
        {
            Debug.LogError("Обе камеры должны быть установлены в ShipController!");
        }
        if (shootPoint == null)
        {
            Debug.LogError("ShootPoint должен быть установлен!");
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
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
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

        cameraPitch = Mathf.Clamp(cameraPitch, -maxCameraAngle, maxCameraAngle);
        cameraYaw = Mathf.Clamp(cameraYaw, -maxYawAngle, maxYawAngle);

        aimingCamera.transform.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
    }

    void Shoot()
    {
        if (shootSound != null && shootSound.clip != null)
        {
            shootSound.Stop();
            shootSound.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource или AudioClip не назначен!");
        }

        if (muzzleFlashPrefab != null)
        {
            GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, shootPoint.position, shootPoint.rotation);
            Destroy(muzzleFlash, 0.5f);
        }

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, shootRange, shootableLayer))
        {
            Debug.Log("Попадание в " + hit.collider.name);

            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
            }

            if (explosionSound != null && explosionSound.clip != null)
            {
                explosionSound.Play();
            }
        }
        else
        {
            Debug.Log("Выстрел в пустоту");
        }
    }
}
