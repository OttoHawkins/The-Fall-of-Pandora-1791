using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float turnSpeed = 20f;
    [SerializeField] private float deceleration = 0.3f;
    [SerializeField] private float stopSpeedThreshold = 0.1f;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private bool isAiming = false;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera aimingCamera;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float cameraRotationSpeed = 2f;
    [SerializeField] private float maxCameraAngle = 30f;
    [SerializeField] private float maxYawAngle = 60f;

    private float cameraPitch = 0f;
    private float cameraYaw = 100f;

    [SerializeField] private float shootRange = 100f;
    [SerializeField] private LayerMask shootableLayer;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource explosionSound;

    private void Start()
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

  private  void Update()
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

   private  void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
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

   private  void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * cameraRotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * cameraRotationSpeed;

        cameraYaw += mouseX;
        cameraPitch -= mouseY;

        cameraPitch = Mathf.Clamp(cameraPitch, -maxCameraAngle, maxCameraAngle);
        cameraYaw = Mathf.Clamp(cameraYaw, -maxYawAngle, maxYawAngle);

        aimingCamera.transform.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
    }

   private  void Shoot()
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

            var hitObject = hit.collider.gameObject;
            var enemyShipController = hitObject.GetComponent<EnemyShipController>();
            if (enemyShipController != null)
            {
                enemyShipController.TakeHit();
            }

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