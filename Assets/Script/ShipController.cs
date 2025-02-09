using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float acceleration = 0.5f;
    public float maxSpeed = 10f;
    public float turnSpeed = 20f;
    public float deceleration = 0.3f;
    public float stopSpeedThreshold = 0.1f;
    public Camera shipCamera;
    public Transform aimModeTransform;
    public float aimTransitionSpeed = 5f; // Скорость перехода камеры в режим прицеливания

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private bool isAiming = false;
    private Vector3 defaultCameraPosition;
    private Quaternion defaultCameraRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1f;
        rb.angularDamping = 3f;

        if (shipCamera != null)
        {
            defaultCameraPosition = shipCamera.transform.localPosition;
            defaultCameraRotation = shipCamera.transform.localRotation;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }

        UpdateCameraPosition();
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

    void UpdateCameraPosition()
    {
        if (shipCamera != null)
        {
            if (isAiming && aimModeTransform != null)
            {
                shipCamera.transform.position = Vector3.Lerp(shipCamera.transform.position, aimModeTransform.position, aimTransitionSpeed * Time.deltaTime);
                shipCamera.transform.rotation = Quaternion.Lerp(shipCamera.transform.rotation, aimModeTransform.rotation, aimTransitionSpeed * Time.deltaTime);
            }
            else
            {
                shipCamera.transform.localPosition = Vector3.Lerp(shipCamera.transform.localPosition, defaultCameraPosition, aimTransitionSpeed * Time.deltaTime);
                shipCamera.transform.localRotation = Quaternion.Lerp(shipCamera.transform.localRotation, defaultCameraRotation, aimTransitionSpeed * Time.deltaTime);
            }
        }
    }
}
